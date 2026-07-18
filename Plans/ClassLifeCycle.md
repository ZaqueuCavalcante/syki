# Ciclo de vida da Turma — da criação à finalização

Plano de gestão do ciclo de vida completo de uma `Class`, cobrindo o que já existe, as lacunas
e a ordem sugerida de implementação. Baseado no estado atual do código (entidades em
`Back/Domain/Classes/`, features em `Back/Features/Classes/`, docs em
`Web/content/docs/3.funcionalidades/6.turma.md`).

## Máquina de estados

```
Pré-matrícula ──release──▶ Matrícula ──(fim do período de matrícula)──▶ Aguardando início*
                                                                              │
                                                                            start
                                                                              ▼
                                              Finalizada ◀──finalize── Iniciada
```

\* `AwaitingStart` é **virtual**: computado em memória quando não há período de matrícula vigente
(o banco continua com `OnEnrollment`). Já tratado em `GetClassesService` e `GetClassService`.

Regras que já valem hoje:

- `OnPreEnrollment → OnEnrollment` (`ReleaseClassForEnrollment`): exige período de matrícula vigente.
- `OnEnrollment → Started` (`StartClass`): exige que **não** haja período de matrícula vigente.
- A partir de `Started` não há retrocesso.

## O que já existe

| Etapa | Feature | Observação |
|---|---|---|
| Criação | `CreateClass` | Nasce simples: disciplina + período + vagas + campus opcional, status `OnPreEnrollment` |
| Professores | `AssignTeachersToClass` | Máx. 2, precisam estar vinculados à disciplina; substitui a lista inteira (serve também para remover) |
| Liberar matrícula | `ReleaseClassForEnrollment` | — |
| Matricular aluno | `AssignStudentToClass` | Exige `OnEnrollment`, valida vaga e duplicidade; aluno entra como `Matriculado` |
| Iniciar | `StartClass` | Só muda o status — **não** gera aulas nem valida montagem |
| Consulta | `GetClass`, `GetClasses` | Com status virtual `AwaitingStart` |
| Chamada | `CreateLessonAttendance` (Teacher) | Marca presenças e finaliza a aula (`ClassLessonStatus.Finalized`) |
| Atividades | `CreateClassActivity` (Teacher) | Peso por `ClassNoteType` (N1/N2/N3), soma ≤ 100 por nota |
| Entrega | `CreateClassActivityWork` (Student) | `AddLink` → `Delivered` |

Peças de domínio prontas mas **sem caller**:

- `Class.CreateLessons()` — gera as aulas a partir dos `Schedules` × datas do `AcademicPeriod`
  (e acumula `Workload`), mas nenhuma feature chama.
- `ClassActivityWork.AddNote(decimal)` — nota 0–10, muda status para `Finalized`, sem endpoint.
- `Schedule.Conflict(other)` — predicado de choque de horários (já usado no plano de Timetabling).
- `FinalizeClassesIn` em `Back/Shared/Features/Academic/` — DTO **legado** (usa `Guid`, mas
  `Class.Id` hoje é `int`); rescrever ou apagar ao implementar a finalização.

## Lacunas, por fase do ciclo

### Fase 1 — Montagem (status `OnPreEnrollment`)

A turma nasce simples e cresce. A ordem natural (e caminho feliz da UI) é
**Professores → Horários → Salas**, porque cada passo é insumo da validação do seguinte:

- **Professores primeiro** — para validar que um horário não gera choque, é preciso saber
  **de quem** é a agenda checada. Horário antes de professor deixa a validação pela metade e o
  erro estoura no momento errado (na atribuição do professor, sem ficar claro o que mudar).
- **Salas por último** — sala é vinculada **por `Schedule`** (sem horário, não há onde pendurar)
  e é opcional (turma online).

**A ordem não deve ser um wizard rígido.** Montagem de semestre é iterativa (professor trocado
no meio, horário renegociado). O desenho certo: cada endpoint valida conflitos contra o que
**já existe** na turma naquele momento, e o `StartClass` é o checkpoint que exige o conjunto
completo e consistente. Concretamente:

- `AssignTeachersToClass` passa a validar a agenda dos novos professores contra os horários já
  definidos da turma;
- `UpdateClassSchedules` valida contra as agendas dos professores já atribuídos;
- `AssignClassroomsToClassSchedules` valida a disponibilidade da sala.

Features da fase:

1. **`UpdateClassSchedules`** — definir/editar os horários da turma (removidos da criação no
   commit `e53a79bb7`, sem substituto até agora). Validações:
   - horários bem formados (`Start < End`) e sem choque entre si (`Schedule.Conflict`);
   - sem choque com outras turmas dos mesmos professores;
   - só editável antes de `Started` (as aulas derivam dos horários).
   - Semântica **replace-all** (manda a lista completa, substitui), como já faz o
     `AssignTeachersToClass` — idempotente e consumível pelo motor de timetabling.
2. **`AssignClassroomsToClassSchedules`** — a pasta já existe **vazia** em
   `Back/Features/Classes/`. Vincular sala (opcional) a cada `Schedule`, validando:
   - sala pertence ao mesmo campus da turma;
   - sala livre no horário (sem `Schedule.Conflict` com outras turmas não finalizadas — exemplo de
     query já documentado no CLAUDE.md).
3. **`UpdateClass`** — editar vagas/campus enquanto `OnPreEnrollment` (vagas não podem ficar
   menores que o nº de matriculados quando editável em `OnEnrollment`).
4. **`DeleteClass`** — permitido apenas em `OnPreEnrollment` (sem alunos, sem histórico).

#### Professor em dois níveis: `Class.Teachers` × `Schedule.TeacherId`

Existem dois modelos de "quem leciona", e os dois ficam — com papéis distintos:

- **`Class.Teachers`** (máx. 2, tabela `ClassTeachers`) — fonte de verdade de *"leciona na
  turma"*: autorização de chamada/atividade (`CreateLessonAttendance` e `CreateClassActivity`
  checam `ClassTeachers`), listagens, docs.
- **`Schedule.TeacherId`** — nível de **alocação**, com papel duplo:
  - em `Schedule` com `ClassId` preenchido: **qual professor da turma cobre aquele slot**, com o
    invariante `Schedule.TeacherId ∈ Class.Teachers`;
  - em `Schedule` com `ClassId` nulo (futuro): **horário preferencial do professor**, como o
    comentário XML da entidade já prevê — insumo de soft constraint do timetabling.

Onde o nível de alocação deixa de ser teórico: **turma com 2 professores**. Se A dá aula na
segunda e B na quarta, validar conflito no nível da turma ("todo professor ocupa todo horário")
é super-restritivo — impediria A de pegar outra turma na quarta, quando na verdade está livre —
e infla as agendas dos professores.

**Faseamento pragmático**: começar validando só no nível da turma (conservador: assume que todo
professor ocupa todo horário — restritivo demais, porém seguro e simples). A alocação por
`Schedule` entra depois como refinamento, preenchida manualmente (turmas com 2 professores) ou
pelo motor de timetabling — que escreve exatamente nesse formato.

#### Compatibilidade com o Timetabling (Plans/Timetabling.md)

As atribuições nascem manuais, mas o **output do timetabling é exatamente o estado que os
endpoints manuais produzem** (professores + schedules com sala/professor por linha). O motor
deve ser um *cliente batch* das mesmas operações de domínio — nunca um caminho paralelo
escrevendo direto no banco. Três decisões de desenho garantem isso:

1. **Semântica replace-all** nos endpoints de montagem (lista completa, substitui) — idempotente,
   o motor re-roda sem deixar estado intermediário.
2. **Predicado de conflito num lugar só** — `Schedule.Conflict` já existe; centralizar as
   queries de choque (agenda de professor, sala, turma) num validator de domínio reutilizável.
   O "esse slot gera conflito?" do greedy first-fit vira literalmente o mesmo código do endpoint
   manual.
3. **Montagem editável até `Started`** — o motor roda por `(Institution, Campus, Period)`,
   gestores ajustam o resultado manualmente e podem re-rodar. Timetabling vira uma fase de
   *preenchimento em lote* da montagem, não um fluxo distinto.

### Fase 2 — Matrícula (status `OnEnrollment`)

5. **`RemoveStudentFromClass`** (cancelar matrícula) — inverso do `AssignStudentToClass`;
   permitido enquanto a turma não for `Started`; libera a vaga.
6. Decidir se `AssignStudentToClass` deve validar **choque de horário** com as outras turmas do
   aluno no mesmo período (os horários agora existem antes da matrícula — dá pra validar).

### Fase 3 — Início (transição para `Started`)

7. **Endurecer `StartClass`** — hoje só troca o status. Passar a exigir:
   - pelo menos 1 professor atribuído;
   - pelo menos 1 horário definido (senão `CreateLessons` gera zero aulas);
   - (decidir) pelo menos 1 aluno matriculado.
8. **Gerar as aulas no `StartClass`** — chamar `Class.CreateLessons()` na transição.
   Resposta à pergunta do TODOS.md ("em que momento criar as aulas?"): **ao iniciar**, porque:
   - os horários ficam congelados a partir daí (não há edição pós-`Started`);
   - as agendas de aluno/professor passam a ter as aulas materializadas de uma vez;
   - chamada sob-demanda complicaria numeração (`Number`) e o cálculo de frequência.
   - Refinamento (pode ficar pra depois): pular dias não letivos usando o calendário acadêmico
     (`Vacation`/`Recess`/`Holiday`) — hoje `CreateLessons` só varre `Period.StartAt..EndAt`.

### Fase 4 — Andamento (status `Started`)

9. **`AddClassActivityWorkNote`** (Teacher) — endpoint para o professor lançar a nota da entrega
   (`ClassActivityWork.AddNote` já pronto no domínio). Validar professor vinculado à turma.
10. **Publicar/finalizar atividade** — `ClassActivityStatus` tem `Pending → Published → Finalized`,
    mas nada transiciona; definir endpoints ou automatizar (ex.: `Finalized` quando todas as
    entregas receberem nota / vencer o prazo).
11. Guard-rails de status: chamadas e atividades só em turma `Started` (hoje
    `CreateLessonAttendance`/`CreateClassActivity` não checam o status da turma).

### Fase 5 — Finalização (transição para `Finalized`)

12. **`FinalizeClass`** — a peça central que falta. Pré-condições:
    - turma `Started`;
    - todas as aulas com chamada feita (`ClassLessonStatus.Finalized`);
    - todas as atividades finalizadas (todas as entregas com nota).
13. **Cálculo do resultado por aluno** ao finalizar, gravando em `ClassStudent.Status`:
    - **Média final**: por `ClassNoteType`, média ponderada das entregas
      (`Σ nota × peso / Σ peso`); depois combinar N1/N2/N3 (decidir fórmula — média simples é o
      ponto de partida). Persistir a média em `ClassStudent` (novo campo `FinalNote`).
    - **Frequência**: presenças / total de aulas; persistir (`Frequency`).
    - **Status final**:
      - frequência < mínimo → `ReprovadoPorFalta` (precede a nota, como na prática brasileira);
      - média < corte → `ReprovadoPorNota`;
      - senão → `Aprovado`.
    - `Dispensado` fica fora deste fluxo (aproveitamento de disciplina — feature própria, depois).
14. **Efeitos colaterais** via sistema de `Command`/`CommandBatch` (padrão do projeto para fluxos
    assíncronos): notificar alunos do resultado, alimentar histórico/insights.
15. **Finalização em lote** — `FinalizeClasses` (o DTO legado indica a intenção); reimplementar
    com `List<int>` reaproveitando o service unitário.

## Decisões tomadas

| Decisão | Resolução |
|---|---|
| Ordem da montagem | Professores → Horários → Salas como caminho feliz da UI; sem wizard rígido — validação incremental em cada endpoint + checkpoint no `StartClass` |
| Papel do `Schedule.TeacherId` | Duplo: professor do slot (`ClassId` preenchido, invariante `∈ Class.Teachers`) e horário preferencial do professor (`ClassId` nulo, futuro input do timetabling) |
| Manual × Timetabling | Endpoints manuais são a API de escrita do timetabling; motor é cliente batch das mesmas operações, com replace-all e predicado de conflito centralizado |

## Decisões em aberto

| Decisão | Opções | Sugestão |
|---|---|---|
| Corte de média e frequência | fixos (7,0 / 75%) × configuráveis por instituição | Começar fixos como constantes; promover a config da instituição depois |
| Fórmula N1/N2/N3 | média simples × pesos por nota × descartar menor | Média simples das notas que tiverem atividades |
| Turma sem atividade em alguma nota | nota vale 0 × nota ignorada na média | Ignorar na média (turma pode usar só N1/N2) |
| Cancelar turma (`Canceled`) | novo status × delete restrito | Novo status terminal, permitido até `Started`; `DeleteClass` só em `OnPreEnrollment` |
| Validar choque de horário na matrícula do aluno | sim × não | Sim, com erro dedicado (`StudentScheduleConflict`) |
| Aulas × calendário acadêmico | gerar tudo × pular dias não letivos | Pular dias não letivos (integra com a feature de calendário já existente) |

## Ordem de implementação sugerida

Cada item é uma feature no padrão do projeto (Controller + Service com validator aninhado +
In/Out `IApiDto` + policy homônima + testes de integração com regions
Authentication/Authorization/Validation errors/Happy path):

1. `UpdateClassSchedules` — destrava agendas, salas e geração de aulas (validando contra as
   agendas dos professores já atribuídos; conflito de professor no nível da turma por enquanto).
2. Validação recíproca no `AssignTeachersToClass` — agenda dos novos professores × horários já
   definidos da turma.
3. `StartClass` endurecido + geração de aulas (`CreateLessons`).
4. `AssignClassroomsToClassSchedules` — preencher a pasta vazia.
5. `AddClassActivityWorkNote` — fecha o ciclo de atividades.
6. `FinalizeClass` (+ cálculo de média/frequência/status do aluno) — apagar/rescrever o
   `FinalizeClassesIn` legado.
7. `RemoveStudentFromClass`, `UpdateClass`, `DeleteClass` — gestão fina.
8. Guard-rails de status nas features de Teacher/Student.
9. Alocação de professor por `Schedule` (turmas com 2 professores) — refinamento que substitui a
   validação conservadora e prepara o formato de escrita do timetabling.
10. Frontend (`Web/app/components/classes/`) acompanhando cada endpoint: horários e salas no
   `DetailManager`, botão de finalizar com resumo de pendências (aulas sem chamada, atividades
   sem nota), tela de resultado por aluno.
11. Docs: atualizar `6.turma.md` e o `TurmaLifecycleDiagram` com a transição de finalização e os
    resultados por aluno.

## Testes-chave por transição

- **Montagem**: horários com choque entre si → erro; horário chocando com outra turma do mesmo
  professor → erro (nos dois sentidos: definindo horário com professor já atribuído **e**
  atribuindo professor com horário já definido); sala ocupada no horário → erro; turma
  `Finalized` não conta como conflito de sala/professor.
- **Release**: sem período de matrícula vigente → `NoCurrentEnrollmentPeriod`; status errado →
  `ClassMustBeOnPreEnrollment` (já cobertos; manter).
- **Start**: sem professor → erro novo; sem horários → erro novo; gera N aulas coerentes com
  horários × dias do período; `Workload` acumulado.
- **Finalize**: aula sem chamada → erro; entrega sem nota → erro; aluno com frequência baixa →
  `ReprovadoPorFalta` mesmo com média alta; média baixa → `ReprovadoPorNota`; caso feliz →
  `Aprovado` + turma `Finalized`; turma finalizada libera sala/horário (queries de conflito já
  filtram `Status != Finalized`).
- **Regressão de status**: qualquer tentativa de retroceder a partir de `Started` falha.
