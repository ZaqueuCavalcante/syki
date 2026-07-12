# Calendário acadêmico — estado atual e decisões em aberto

Documento de design da feature de calendário. A primeira parte descreve o que já está implementado
e **por que**; a segunda registra os pontos discutidos que ainda não viraram código.

## O que já existe

### Endpoints

| Endpoint | Policy | Descrição |
|---|---|---|
| `GET /calendar/institution?year=` | `GetInstitutionCalendar` | Todos os dias do ano, com o tipo de cada um |
| `POST /calendar/days` | `CreateCalendarDay` | Customiza um dia |
| `PUT /calendar/days` | `UpdateCalendarDay` | Edita um dia já customizado |

Todas exigem `UserType.Manager` + `EstudPermissions.ManageCalendar`.

### `DayType`

`Default` (dia letivo), `Vacation` (férias), `Recess` (recesso), `Holiday` (feriado), `Weekend` (fim de semana).

**Férias × recesso.** Férias são do aluno *e* do servidor: a instituição para, o professor está de
férias e não pode ser convocado. Recesso suspende as aulas mas não as atividades da instituição — o
professor pode estar trabalhando (fechando notas, planejando o semestre) e o recesso não desconta do
direito a férias de ninguém. Hoje os dois têm o mesmo efeito prático (dia não letivo), mas o calendário
oficial precisa distinguir, e a diferença vira comportamento no dia em que o sistema responder
"esse professor pode ser escalado?" — em recesso pode, em férias não.

### Modelo: derivar, não materializar

A tabela `estud.calendar_days` é **esparsa**: só existe linha para o dia que a instituição
customizou. O resto é derivado em tempo de request, nesta ordem de precedência:

```
dia customizado  →  feriado nacional  →  fim de semana  →  dia letivo
```

Feriados nacionais (fixos e móveis, com cálculo da Páscoa) vivem em `Back/Domain/Calendar/NationalHolidays.cs`.

**Por que não criar os 365 dias quando a instituição é criada:** o sistema já sabe derivar tudo isso,
então persistir seria guardar informação redundante — e informação redundante envelhece.
Materializar custaria ~365 linhas × instituições × anos (com 500 instituições, ~180 mil linhas/ano só
pra dizer "esse é um dia comum"), exigiria um job anual pra gerar o ano seguinte (senão em 01/01 o
calendário aparece vazio) e, quando a lista de feriados mudasse (lei nova ou correção de bug no cálculo
de Corpus Christi), as instituições antigas ficariam com linhas erradas congeladas, precisando de
migração de dados. No modelo derivado, corrige-se `NationalHolidays.cs` e todo mundo vê certo no
request seguinte.

O gestor não percebe diferença: o GET devolve os 365 dias do mesmo jeito. O banco só carrega o que é
**decisão** da instituição.

### Frontend

Página `/calendar` (item "Calendário" na sidebar, visível só para Manager com `ManageCalendar`):
12 meses em grade, navegação de ano, legenda com o total de cada tipo, e clique no dia abrindo o modal
de edição. O modal decide entre `POST` e `PUT` pelo `Id` do item, que vem nulo quando o dia ainda não
foi customizado.

---

## Decisões em aberto

### 1. Calendário por campus

**Necessidade real**, não hipótese: instituição com campus em Recife e em Caruaru tem 24/06 (São João)
como feriado só em Caruaru. Aniversário de cidade, padroeiro e ponto facultativo estadual seguem a mesma
lógica. Campi também costumam ter semana de provas e recesso próprios.

**O que não fazer:** trocar `InstitutionId` por `CampusId` no `CalendarDay`. Perde-se o caso mais comum
(o recesso de fim de ano vale para a instituição inteira) e cada decisão passa a ser replicada em N campi,
com risco de divergirem por descuido.

**Proposta — camadas.** `CalendarDay` ganha `int? CampusId`: nulo = vale para toda a instituição,
preenchido = exceção daquele campus. A precedência vira:

```
dia do campus  →  dia da instituição  →  feriado nacional  →  fim de semana  →  dia letivo
```

É a mesma ideia de override que já existe, com um nível a mais, e é aditivo — `CampusId` nulo preserva
os dados e o comportamento atuais.

Consequências:

- **Índice único** passa a ser `(institution_id, campus_id, date)`. Pegadinha do Postgres: `NULL` não
  colide com `NULL` num índice único, então dois registros de instituição para a mesma data passariam
  batido. Precisa de `NULLS NOT DISTINCT` (PG 15+) ou de dois índices parciais
  (`WHERE campus_id IS NULL` e `WHERE campus_id IS NOT NULL`).
- **API:** `GET /calendar/institution` ganha `campusId` opcional — sem ele, o calendário base da
  instituição; com ele, o calendário efetivo daquele campus, já com as exceções aplicadas.
- **UI:** select de campus no topo ("Toda a instituição" como padrão). O modal precisa deixar claro em
  qual camada está escrevendo, senão o gestor marca um feriado achando que é local e derruba a aula em
  todos os campi.

**Quando fazer:** dá pra esperar a primeira instituição multi-campus reclamar de feriado municipal.
O `int? CampusId` entra depois sem migração dolorosa (coluna nullable + troca do índice), justamente
porque o modelo é derivado e não há 365 linhas por campus pra reprocessar.

### 2. Eventos no dia

Um mesmo dia pode ter vários eventos (uma apresentação para os cursos de engenharia e uma feira para a
licenciatura, no mesmo dia).

**Tabela nova, mas não apontando para `calendar_days`** — apontando para uma data. Como `calendar_days`
é esparsa, uma FK `calendar_day_id` obrigaria a materializar um dia só pra pendurar o evento nele
(criando linhas de dia sem nenhuma decisão de calendário por trás), e o reset do dia viraria uma
armadilha para o evento.

Os dois conceitos têm cardinalidade e natureza diferentes:

- **`DayType` é o regime do dia.** Exclusivo e único — um dia é letivo, ou férias, ou recesso, ou
  feriado. Responde: "conta carga horária? tem chamada?".
- **Evento é um fato que acontece.** 0..N por dia, aditivo, e na maioria das vezes **não muda o regime**
  (a apresentação acontece numa terça letiva normal). Responde: "o que tem marcado nesse dia?".

Esboço:

```
calendar_events
  id | institution_id | campus_id (null = toda a instituição)
  start_date | end_date | title | description
```

**Intervalo, não dia único:** feira de licenciatura dura três dias, semana de provas dura cinco. Modelar
como intervalo evita replicar linhas idênticas e ter que editá-las uma a uma. O GET faz um range query
no ano e distribui os eventos nos dias que eles cobrem — mesma lógica de derivação já usada.

**Público-alvo:** tabela de junção (`calendar_event_courses`, ou `calendar_event_targets` se um dia
quiser mirar em turma ou tipo de usuário), com a regra "sem nenhum alvo = evento da instituição inteira".
Sem isso, o aluno de licenciatura vê a apresentação de engenharia no calendário dele e o calendário vira
ruído.

**Impacto:** `GetInstitutionCalendarItemOut` ganha uma `List<EventOut> Events` ao lado do `DayType`; a UI
pinta o dia pelo regime e marca a existência de eventos com um ponto/contador, abrindo a lista no clique.
Endpoints: `CreateCalendarEvent` / `UpdateCalendarEvent` / `DeleteCalendarEvent`.

---

## Nota sobre migrations

O repositório **não tem migrations versionadas** (`Back/Migrations` não existe; os testes criam o banco
com `EnsureCreatedAsync`). Rodar `dotnet ef migrations add` hoje geraria uma migration inicial com o
schema inteiro, não só a tabela nova. Para prod, o DDL da `calendar_days` vai na mão:

```sql
CREATE TABLE estud.calendar_days (
    id              integer GENERATED BY DEFAULT AS IDENTITY,
    institution_id  integer NOT NULL,
    date            date    NOT NULL,
    day_type        integer NOT NULL,
    description     text    NULL,
    CONSTRAINT pk_calendar_days PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_calendar_days_institution_id_date
    ON estud.calendar_days (institution_id, date);
```

`day_type` é `integer`: 0 = Default, 1 = Vacation, 2 = Recess, 3 = Holiday, 4 = Weekend.
