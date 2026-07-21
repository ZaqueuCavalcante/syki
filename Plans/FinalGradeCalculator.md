# Cálculo da Nota Final — avaliações e regra de fechamento por instituição

Plano para tornar configurável, por instituição, **quais avaliações uma turma tem** e **qual a
regra que combina essas avaliações na nota final**. Hoje isso é um enum fixo (`ClassNoteType`:
N1/N2/N3) e o cálculo de média ainda é *mock*. Este plano é a peça que falta pra fechar a
**Fase 5** do [ClassLifeCycle.md](./ClassLifeCycle.md) (item 13, "Cálculo do resultado por
aluno"), promovendo as decisões que lá ficaram em aberto (fórmula N1/N2/N3, corte de nota) a um
modelo de primeira classe.

## Motivação

A nota mínima (`NoteLimit`) e a frequência mínima (`FrequencyLimit`) já são config da instituição
(`InstitutionConfig`), mas são **escalares** — fáceis de promover. As regras de avaliação vão
muito além disso, e variam de verdade entre instituições:

- **Instituição A** — 3 notas (N1, N2, N3); nota final = **média das duas maiores** (descarta a menor).
- **Instituição B** — 2 notas regulares + recuperação; nota final = **`max( média(N1,N2), Rec )`**
  ("fica a maior" entre a média das duas e a recuperação).

Isso deixa claro que há **três eixos de variação distintos**, não um só:

| Eixo | O que varia | Hoje |
|---|---|---|
| **Quais avaliações** | quantidade, nome, ordem (N1/N2/N3, bimestres, P1/P2...) | enum fixo `ClassNoteType` |
| **Papel da avaliação** | regular × recuperação/prova final | inexistente |
| **Regra de fechamento** | como as notas viram a média final | inexistente (média é `random.NextDouble()`) |

## Estado atual do código

| Peça | Arquivo | Situação |
|---|---|---|
| Enum das notas | `Back/Shared/Enums/ClassNoteType.cs` | `N1, N2, N3` fixos |
| Config da instituição | `Back/Domain/Institutions/InstitutionConfig.cs` | só `NoteLimit` (7,0) e `FrequencyLimit` (70%) |
| Nota da atividade | `Back/Domain/Classes/ClassActivity.cs:10` | `ClassNoteType Note` (enum) |
| Nota do aluno na turma | `Back/Domain/Students/StudentClassNote.cs:11` | `ClassNoteType Type` (enum), índice único `(ClassId, StudentId, Type)` |
| Status final do aluno | `Back/Shared/Enums/StudentClassStatus.cs` | já tem `Aprovado`, `ReprovadoPorNota`, `ReprovadoPorFalta` |
| Média (mock) | `GetClassService.cs:57`, `GetStudentDetailsService.cs` | `random.NextDouble()` — **não há lógica real ainda** |

**Observação que define a prioridade:** como o fechamento ainda é *mock*, dá pra nascer já com o
modelo certo em vez de acoplar a lógica real aos três valores fixos. Fazer isso *depois* que a
média real existir é bem mais caro (migration de dados + reescrita do cálculo).

## Decisão de design central: catálogo de estratégias, **não** motor de fórmulas

O reflexo é armazenar uma expressão/fórmula por instituição e interpretar em runtime. **Rejeitado**:
vira um mini-DSL (parsing, validação, arredondamento, divisão por zero, segurança) impossível de
testar com confiança, e abre caminho pra uma instituição configurar uma fórmula que quebra o
fechamento em produção.

O conjunto real de regras acadêmicas brasileiras é **pequeno e finito**. Então: **catálogo de
estratégias codadas** (strategy pattern).

- A instituição **escolhe** uma regra de um catálogo fechado (`FinalGradeRule`).
- Cada regra é respaldada por um `IFinalGradeCalculator` **codado e testado**.
- Parâmetros estruturados quando a regra precisa (ex.: `keep`, `combine`, arredondamento).

**Trade-off assumido:** regra nova = código novo + deploy. Em troca: type-safety, testes de
verdade, zero risco de fórmula inválida em produção. Aceitável — regras acadêmicas mudam raramente.

## Modelo de domínio

### 1. `EvaluationPeriod` (nova entidade owned da instituição)

Substitui o enum `ClassNoteType`. Representa uma avaliação configurável (N1, N2, recuperação...).

```
EvaluationPeriod
  Id            int
  InstitutionId int
  Order         int                 // ordem de exibição/lançamento
  Name          string              // "N1", "Bimestre 1", "Recuperação"...
  Role          EvaluationRole       // Regular | Recovery
  Weight        decimal?            // usado só pelas regras ponderadas
```

- `EvaluationRole` (novo enum): `Regular`, `Recovery`. Resolve o "OU a última toda" sem hardcode
  de posição — a estratégia separa notas regulares da recuperação pelo papel.
- Criada com um **default N1/N2/N3 (todas `Regular`)** no momento em que a instituição nasce
  (`Institution.cs:52`, hoje `Config = new InstitutionConfig()`), preservando o comportamento atual.

### 2. `InstitutionConfig` — ganha a regra de fechamento

```
InstitutionConfig  (+ campos)
  FinalGradeRule        FinalGradeRule    // enum: qual estratégia
  FinalGradeRuleParams  string (jsonb)    // { keep, combine, rounding... }
```

`FinalGradeRule` (novo enum, o catálogo):

| Valor | Regra | Params | Cobre |
|---|---|---|---|
| `SimpleAverage` | média simples de todas as regulares | — | default atual |
| `WeightedAverage` | média ponderada por `EvaluationPeriod.Weight` | — | pesos por nota |
| `BestNAverage` | média das N maiores regulares | `{ keep: int }` | **Instituição A** (`keep=2`) |
| `RegularAvgOrRecovery` | `combine( média(regulares), recuperação )` | `{ combine: Max\|Replace }` | **Instituição B** (`combine=Max`) |

### 3. FKs no lugar do enum

- `ClassActivity.Note` (`ClassNoteType`) → `ClassActivity.EvaluationPeriodId` (FK).
- `StudentClassNote.Type` (`ClassNoteType`) → `StudentClassNote.EvaluationPeriodId` (FK).
  Índice único passa a ser `(ClassId, StudentId, EvaluationPeriodId)` — hoje
  `(ClassId, StudentId, Type)` em `StudentClassNoteDbConfig.cs:20`.

## A interface do cálculo

```csharp
public interface IFinalGradeCalculator
{
    FinalGradeRule Rule { get; }
    // notas do aluno já resolvidas por avaliação (valor + papel + peso)
    decimal Calculate(IReadOnlyList<EvaluationNote> notes, FinalGradeRuleParams config);
}
```

Uma implementação por valor do catálogo, resolvida por `Rule` (DI keyed service ou dicionário).
Cada uma é uma classe pequena, pura e 100% testável em unit test, **sem** tocar banco.

Mapeamento dos exemplos do usuário:

- **A** → `BestNAverage { keep = 2 }`: ordena as 3 regulares desc, tira média das 2 maiores.
- **B** → `RegularAvgOrRecovery { combine = Max }`: `max( média(N1,N2), Rec )`. Se não houver
  nota de recuperação lançada, cai na média das regulares.

## Onde pluga: `FinalizeClass`

O `IFinalGradeCalculator` é chamado no cálculo do resultado por aluno da **Fase 5** do
`ClassLifeCycle.md` (item 13). O fluxo por aluno na finalização:

1. Média **de cada avaliação** — média ponderada das entregas daquela avaliação
   (`Σ nota × peso / Σ peso`), material que hoje o `ClassActivity.Weight` já modela.
2. **Nota final** — `IFinalGradeCalculator.Calculate(...)` combina as avaliações conforme a regra
   da instituição. Persistir em `ClassStudent` (novo campo `FinalNote`).
3. **Status final** (usa `InstitutionConfig`, ordem da prática brasileira):
   - frequência `< FrequencyLimit` → `ReprovadoPorFalta` (precede a nota);
   - nota final `< NoteLimit` → `ReprovadoPorNota`;
   - senão → `Aprovado`.

A regra de fechamento fica **isolada do cálculo de frequência e do corte** — o calculator só
produz a nota final; corte/frequência continuam decisão do `FinalizeClass`.

## Features (padrão do projeto)

1. **`SetupEvaluationPeriods`** (nova, Adm/Institutions) — definir a lista de avaliações da
   instituição (replace-all: nome, ordem, papel, peso). Validar: ≥ 1 regular, nomes não vazios
   (`HasValue()`), no máx. 1 recuperação por enquanto, pesos coerentes com a regra escolhida.
2. **`SetupInstitutionConfig`** (existente) — estender `In`/`Out`/`Mapper`/`Validator` com
   `FinalGradeRule` + params. Validar que os params batem com a regra (`BestNAverage` exige
   `keep ≥ 1` e `keep ≤ nº de regulares`; `RegularAvgOrRecovery` exige que exista uma avaliação
   `Recovery`). Novos erros em `Back/Errors/EstudErrors.Institutions.cs` no padrão de
   `InvalidNoteLimit`/`InvalidFrequencyLimit`.
3. **`GetInstitutionConfig`** — expor a regra + params + a lista de avaliações.
4. **`CreateClassActivity`** (Teacher) — o `In` passa a referenciar `EvaluationPeriodId` (validar
   que pertence à instituição) em vez de `ClassNoteType`.
5. **Consumo do calculator no `FinalizeClass`** (ainda a implementar no ciclo de vida).

## Migration

Mudança estrutural em duas tabelas + config. Passos:

1. Criar `evaluation_periods` e semear **N1/N2/N3 `Regular`** para cada instituição existente.
2. `institution_configs`: adicionar `final_grade_rule` (default `SimpleAverage`) e
   `final_grade_rule_params` (jsonb, default `{}`).
3. `class_activities` / `student_class_notes`: adicionar `evaluation_period_id`, **backfill**
   casando o enum antigo (`N1/N2/N3`) com o `EvaluationPeriod` de mesmo nome da instituição da
   turma; depois dropar as colunas enum e recriar o índice único de `student_class_notes` sobre
   `(class_id, student_id, evaluation_period_id)`.
4. Remover `ClassNoteType` do código após a migration de dados (o enum deixa de existir).

> Migrations rodam no Windows (`dotnet ef migrations add FinalGradeCalculator`); ver comandos no
> CLAUDE.md. **Não** rodar `dotnet build` aqui.

## Decisões tomadas

| Decisão | Resolução |
|---|---|
| Fórmula configurável | Catálogo fechado de estratégias (`FinalGradeRule` + `IFinalGradeCalculator`), **não** motor de fórmulas |
| Avaliações | Entidade owned `EvaluationPeriod` (nome + ordem + papel + peso), substitui o enum `ClassNoteType` |
| Recuperação | Modelada como `EvaluationRole.Recovery`, não como posição/última nota |
| Momento de decidir | Agora, enquanto a média ainda é mock — evita migration de dados sobre lógica real |
| Corte de nota/frequência | Permanecem escalares em `InstitutionConfig`; o calculator não decide aprovação, só a nota final |

## Decisões em aberto

| Decisão | Opções | Sugestão |
|---|---|---|
| Regras do catálogo v1 | quais entram já | `SimpleAverage` (default) + `BestNAverage` + `RegularAvgOrRecovery` cobrem os dois exemplos; `WeightedAverage` se houver demanda |
| Mais de uma recuperação | 1 recuperação × N recuperações (por avaliação) | Começar com 1 recuperação por turma; generalizar depois |
| Avaliação sem nota lançada | conta 0 × ignora na média | Ignorar (turma pode usar só N1/N2), como já sugerido no ClassLifeCycle.md |
| Arredondamento | truncar × arredondar × casas configuráveis | Parâmetro no `FinalGradeRuleParams` (default: arredondar a 2 casas) |
| Config por curso/disciplina | só instituição × override por curso | Só instituição no v1; override é evolução futura |

## Ordem de implementação sugerida

Cada item é uma feature no padrão do projeto (Controller + Service com validator aninhado + In/Out
`IApiDto` + policy homônima + testes de integração com regions
Authentication/Authorization/Validation errors/Happy path).

1. **`EvaluationPeriod`** — entidade + `IEntityTypeConfiguration` + default N1/N2/N3 no nascimento
   da instituição + migration com seed. Ainda **sem** trocar as FKs (coexiste com o enum).
2. **`IFinalGradeCalculator` + estratégias** — puro domínio, unit tests cobrindo A e B, sem banco.
   Entregável isolado e testável antes de qualquer mudança de schema nas tabelas de nota.
3. **`InstitutionConfig` + `SetupInstitutionConfig`/`GetInstitutionConfig`** — regra + params,
   validação regra×params, novos erros.
4. **`SetupEvaluationPeriods`** — CRUD replace-all das avaliações da instituição.
5. **Troca das FKs** — `ClassActivity` e `StudentClassNote` passam a apontar pra `EvaluationPeriod`;
   `CreateClassActivity` ajustado; migration de backfill; remover `ClassNoteType`.
6. **Plug no `FinalizeClass`** — consumir o calculator no cálculo do resultado por aluno (fecha a
   Fase 5 do ClassLifeCycle.md).
7. **Frontend** — tela de config da instituição (regra + avaliações), abas dinâmicas de nota
   (hoje N1/N2/N3 fixas) em atividades e boletim.

## Testes-chave

- **Calculator (unit)** — `BestNAverage(keep=2)` com notas `[9, 5, 8]` → `8.5`; `RegularAvgOrRecovery(Max)`
  com `média(6,7)=6.5` e `Rec=8` → `8`; com `Rec` ausente → `6.5`; empates e listas incompletas.
- **Setup config** — regra `BestNAverage` com `keep` > nº de regulares → erro; `RegularAvgOrRecovery`
  sem avaliação `Recovery` cadastrada → erro; params incoerentes → erro dedicado.
- **Setup avaliações** — zero regulares → erro; nome vazio (`IsEmpty()`) → erro; replace-all
  substitui a lista inteira.
- **Finalização (integração)** — instituição A e B configuradas de forma diferente produzem notas
  finais diferentes pros mesmos lançamentos; frequência baixa → `ReprovadoPorFalta` mesmo com nota
  alta; nota final `< NoteLimit` → `ReprovadoPorNota`; caso feliz → `Aprovado`.
- **Migration** — dados N1/N2/N3 existentes casam com os `EvaluationPeriod` semeados; índice único
  de `student_class_notes` preservado sobre a nova coluna.
