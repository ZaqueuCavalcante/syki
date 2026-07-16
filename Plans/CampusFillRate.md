# Remover Campus.Capacity + Visualização de Ocupação do Campus

## Contexto

`Campus.Capacity` é conceitualmente errado: um campus tem várias salas que operam em turnos (manhã/tarde/noite), então "capacidade de alunos do campus" não é uma grandeza física verificável — e o `FillRate` calculado a partir dela (matrículas ÷ capacidade) compara grandezas incomparáveis. A capacidade física real já existe em `Classroom.Capacity`.

Este plano: (A) remove `Capacity`/`FillRate` do Campus em todo o stack; (B) cria a feature de alocar salas aos horários das turmas — pré-requisito, pois hoje **nada preenche `Schedule.ClassroomId`**; (C) cria o endpoint + página de ocupação do campus: um grid dia da semana × turno mostrando a taxa de uso das salas, com drilldown por sala e taxa geral (macro).

**Decisões:**
- Métrica = **tempo de uso**: minutos agendados ÷ minutos disponíveis (salas × duração do turno)
- Turnos contíguos: Matutino [07:00–12:00) = 300min, Vespertino [12:00–18:00) = 360min, Noturno [18:00–24:00) = 360min; horário que cruza fronteira contribui com os minutos clipados em cada turno
- Contam turmas com `Status != Finalized`
- UI: **página dedicada** `/campus/[id]`
- Vínculo sala↔horário **incluído no escopo** (Fase B)

---

## Fase A — Remover `Capacity` do Campus

### Backend
- `Back/Domain/Campi/Campus.cs` — remover propriedade, parâmetro do ctor e do `Update`
- `Back/Errors/EstudErrors.Campus.cs` — deletar `InvalidCampusCapacity` (NÃO tocar `InvalidClassroomCapacity`)
- `Back/Features/Campi/CreateCampus/` — `In` (campo + exemplos), `Service` (RuleFor + arg do `new Campus`), `Controller` (ErrorsExamples)
- `Back/Features/Campi/UpdateCampus/` — `In`/`Out` (campos + exemplos), `Service` (RuleFor + arg do `Update`), `Mapper`, `Controller`
- `Back/Features/Campi/GetCampi/` — `GetCampiOut` (campos `Capacity` e `FillRate` + exemplos), `GetCampiMapper.cs` (linhas `Capacity =` e `FillRate =`). `GetCampiService` não muda.
- `CampusDbConfig.cs` não precisa de edit (mapeado por convenção). **Usuário roda** `dotnet ef migrations add RemoveCampusCapacity`.

### Testes
- `Tests/Clients/TestsHttpClient.Campi.cs` — remover param `int capacity` de `CreateCampus`/`UpdateCampus`
- `Tests/Features/Campi/CreateCampus/CreateCampusIntegrationTests.cs` — deletar teste de capacidade inválida; remover args posicionais de capacity
- `Tests/Features/Campi/UpdateCampus/UpdateCampusIntegrationTests.cs` — deletar teste de capacidade inválida; remover `updated.Capacity.Should().Be(789)`; ajustar args
- `Tests/Features/Campi/GetCampi/GetCampiIntegrationTests.cs` e `Tests/Features/Dev/DevIntegrationTests.cs` (linhas 33–36) — só ajustar args posicionais
- Grep final por `capacity` nos testes pra pegar sobras

### Frontend
- `Web/app/pages/campi.vue` — remover `capacity`/`fillRate` da interface, stat `/{{ campus.capacity }}`, barra + badge de ocupação, helpers `fillRateColor`/`fillRateBadgeColor`
- `Web/app/components/campi/CreateModal.vue` e `EditModal.vue` — remover campo zod `capacity`, toda a maquinaria de input mascarado (`capacityDisplay`, `formatCapacity`, `onCapacityKeydown`, `onCapacityInput`) e o `UFormField "Capacidade"`
- NÃO tocar em classroom capacity (`classrooms/*`)

### Docs
- `Web/content/docs/3.funcionalidades/4.campus.md` — remover linha Capacidade da tabela, ajustar passo de criação, trocar bullet "Capacidade" por um sobre ocupação por dia/turno
- `Web/app/components/docs/CampusDiagram.global.vue` — remover `capacity` do mock e o badge `"X vagas"`

---

## Fase B — Alocar salas aos horários da turma

Vínculo vivo = `Schedule.ClassroomId` (já lido por `GetClassroomService`). Ignorar a entidade `ClassroomClass` (configurada mas sem DbSet e sem uso). Sem mudança de schema.

### B1. Expor schedules com Id/sala no GetClass
- `Back/Features/Classes/GetClass/GetClassOut.cs` — adicionar `CampusId` e trocar `List<ScheduleOut>` por DTO local `GetClassScheduleOut { Id, Day, StartAt, EndAt, ClassroomId?, Classroom }` (não mutar o `ScheduleOut` compartilhado)
- `GetClassService.cs` — buscar nomes das salas dos `ClassroomId`s não nulos e mapear

### B2. Nova feature `Back/Features/Classes/AssignClassroomsToClassSchedules/`
Endpoint: `PUT classes/{id}/schedules/classrooms` — lista completa, idempotente:
```csharp
public class AssignClassroomsToClassSchedulesIn : IApiDto<...>
{
    public List<AssignClassroomsToClassSchedulesItemIn> Assignments { get; set; } = [];
}
public class AssignClassroomsToClassSchedulesItemIn
{
    public int ScheduleId { get; set; }
    public int? ClassroomId { get; set; }   // null = desalocar
}
```
Out = `SuccessOut` (padrão do StartClass). Service (ordem):
1. Validator: lista não vazia e sem `ScheduleId` duplicado → `InvalidClassroomAssignments`
2. Carregar turma + `Include(Schedules)` com escopo de instituição → `ClassNotFound.I`
3. `Status == Finalized` → `ClassIsFinalized.I`
4. Todo `ScheduleId` deve pertencer à turma → `ScheduleNotFound.I`
5. Alocando sala com `Class.CampusId == null` → `ClassWithoutCampus.I`
6. Salas: existência/instituição → `ClassroomNotFound.I` (já existe); campus diferente da turma → `ClassroomNotInClassCampus.I`
7. **Conflito**: carregar schedules de outras turmas não finalizadas nas salas alvo; `others.Any(o => o.ClassroomId == a.ClassroomId && o.Conflict(schedule))` → `ClassroomScheduleConflict.I` (`Schedule.Conflict()` já cobre dia + sobreposição). Sem bloqueio por capacidade de assentos (métrica é tempo).
8. Aplicar `schedule.ClassroomId = ...`, `SaveChangesAsync`, retornar `SuccessOut`

### B3. Novos erros (`Back/Errors/EstudErrors.Classes.cs`, template singleton)
`ScheduleNotFound`, `ClassIsFinalized`, `ClassWithoutCampus`, `ClassroomNotInClassCampus`, `ClassroomScheduleConflict`, `InvalidClassroomAssignments` — mensagens em pt-BR.

### B4. Policy
`Back/Auth/Policies/Policies.Classes.cs` — const `AssignClassroomsToClassSchedules` + `.AddEstudPolicy(..., UserType.Manager, EstudPermissions.ManageClasses)`.

### B5. Frontend
- `Web/app/policies/types.ts` + `store.ts` — adicionar a policy (Manager + `ManageClasses`)
- `Web/app/types/classes.ts` — `ClassSchedule` ganha `id`, `classroomId?`, `classroom?`; `GetClassOut` ganha `campusId`
- **Novo** `Web/app/components/classes/AssignClassroomsModal.vue` — `UModal` padrão; uma linha por horário (`formatClassSchedule`) com `USelect` de salas (fetch `GET /classrooms`, filtrado por `campusId` da turma, opção "Sem sala" = null, label `nome (capacidade)`); submit `PUT .../classes/${id}/schedules/classrooms`; handlers em arrow function; toast + emit `updated`
- `Web/app/components/classes/DetailManager.vue` — seção de horários mostra sala ou badge "Sem sala"; botão "Alocar salas" gated por `can('AssignClassroomsToClassSchedules')`, oculto se `Finalized` ou sem campus

### B6. Testes
- `TestsHttpClient.Classes.cs` — `AssignClassroomsToClassSchedules(int classId, List<...ItemIn>)` via `PutAsJsonAsync` + `Resolve<SuccessOut>()`
- Novo `Tests/Features/Classes/AssignClassroomsToClassSchedules/...IntegrationTests.cs` com regions: Authentication (401), Authorization (403), Validation errors (lista vazia, scheduleId duplicado, turma inexistente/de outra instituição, schedule de outra turma, turma sem campus, sala de outra instituição, sala de outro campus, conflito de horário), Happy path (alocar → `GetClass` reflete; realocar; desalocar com null; mesma sala em horários não sobrepostos)

---

## Fase C — Ocupação do campus (endpoint + página)

### C1. Feature `Back/Features/Campi/GetCampusOccupancy/` — `GET campi/{id:int}/occupancy`
EF LINQ + cálculo em memória (volume pequeno; clipping de minutos é mais legível/testável em C# que em SQL).

```csharp
public class GetCampusOccupancyOut : IApiDto<GetCampusOccupancyOut>
{
    public int CampusId { get; set; }
    public string Campus { get; set; }
    public int TotalClassrooms { get; set; }
    public decimal OverallRate { get; set; }                          // 0–100, 2 casas
    public List<GetCampusOccupancyCellOut> Cells { get; set; } = [];  // 18 = 6 dias × 3 turnos, sempre presentes
}
public class GetCampusOccupancyCellOut
{
    public Day Day { get; set; }
    public CourseSession Shift { get; set; }   // reusa Morning/Afternoon/Evening (Matutino/Vespertino/Noturno)
    public int AvailableMinutes { get; set; }  // salas × duração do turno
    public int UsedMinutes { get; set; }
    public decimal Rate { get; set; }
    public List<GetCampusOccupancyClassroomOut> Classrooms { get; set; } = []; // TODAS as salas (inclui 0%) → drilldown sem 2ª request
}
public class GetCampusOccupancyClassroomOut
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UsedMinutes { get; set; }
    public decimal Rate { get; set; }
}
```

Service:
```csharp
private static readonly (CourseSession Shift, int Start, int End)[] Shifts =
[
    (CourseSession.Morning,    7 * 60, 12 * 60),
    (CourseSession.Afternoon, 12 * 60, 18 * 60),
    (CourseSession.Evening,   18 * 60, 24 * 60),
];
private static int ToMinutes(Hour h) { var v = (int)h; return v / 100 * 60 + v % 100; } // enum é HHMM
```
- Campus com escopo de instituição → `CampusNotFound.I`; salas do campus ordenadas por nome
- Schedules: `ClassroomId != null` nas salas do campus, join com `Classes` onde `Status != Finalized`
- Por célula (Day × Shift): por sala, `used = Max(0, Min(endMin, shiftEnd) - Max(startMin, shiftStart))` somado; `Rate = available > 0 ? Round(100m * used / available, 2) : 0`
- `OverallRate = Σ used / Σ available` das 18 células; campus sem salas → tudo 0, células emitidas mesmo assim
- Controller: `[Authorize(Policies.GetCampusOccupancy)]`, `ErrorsExamples : ErrorExamplesProvider<CampusNotFound>`, summary/remarks multi-linha pt-BR explicando a métrica

### C2. Policy
`Policies.Campi.cs` — const `GetCampusOccupancy` + `.AddEstudPolicy(..., UserType.Manager, EstudPermissions.ManageCampi)`.

### C3. Testes
- `TestsHttpClient.Campi.cs` — `GetCampusOccupancy(int id)` → `Resolve<GetCampusOccupancyOut>()`
- Novo `Tests/Features/Campi/GetCampusOccupancy/...IntegrationTests.cs`: Authentication, Authorization, Validation (`CampusNotFound` — id inexistente e campus de outra instituição), Happy path:
  - campus sem salas → 18 células zeradas, `TotalClassrooms = 0`
  - 1 sala + horário seg 07:00–10:00 alocado (via endpoint da Fase B) → célula seg/Matutino `UsedMinutes = 180`, `Rate = 60`; demais 0; `OverallRate = Round(100m * 180 / (1020 * 6), 2)`
  - horário 11:00–13:00 cruzando fronteira → 60min no Matutino + 60min no Vespertino
  - schedule sem sala não conta; 2ª sala dobra o disponível e halva a taxa

### C4. Frontend — página dedicada
- `Web/app/pages/campi.vue` **fica onde está** (sem mover). A página de detalhe entra como **`Web/app/pages/campus/[id].vue`** (rota `/campus/:id`, singular) — evita que o Nuxt transforme `campi.vue` em rota-pai exigindo `<NuxtPage>`
- `Web/app/middleware/policy.global.ts` — adicionar branch `to.path.startsWith('/campus/')` → policy `GetCampusOccupancy` (espelho do `isClassroomDetail`)
- `policies/types.ts` + `store.ts` — `GetCampusOccupancy` (Manager + `ManageCampi`)
- **Promover constantes compartilhadas** em `Web/app/utils/classes.ts`: exportar `dayLabels` (hoje privado), adicionar `weekDays` (`{key,label,short}`, 6 dias, de `agenda/Week.vue`) e labels de turno (`Morning: 'Matutino'`...); refatorar `agenda/Week.vue`, `classes/CreateModal.vue` e `course-offerings/CreateModal.vue` pra importar
- `Web/app/pages/campus/[id].vue` (modelo: `classrooms/[id].vue` — `useFetch` com `credentials: 'include', server: false`, navbar com seta de volta, spinner/erro):
  - Stats: taxa geral (macro), total de salas, totais por turno
  - **Heatmap**: CSS grid discreto `grid-cols-[auto_repeat(6,1fr)]`; header = `weekDays.short` (coluna de hoje destacada como no `Week.vue`); 3 linhas Matutino/Vespertino/Noturno; célula = `<button>` com `rate%`, cor em rampa sequencial de intensidade do primary (ocupação baixa não é "ruim" — nada de vermelho/verde):
    ```ts
    function cellClass(rate: number): string {
      if (rate === 0) return 'bg-elevated text-muted'
      if (rate < 20) return 'bg-primary/10'
      if (rate < 40) return 'bg-primary/25'
      if (rate < 60) return 'bg-primary/40'
      if (rate < 80) return 'bg-primary/60 text-inverted'
      return 'bg-primary/80 text-inverted'
    }
    ```
    Célula selecionada com `ring-2 ring-primary`; click em arrow function (regra do CLAUDE.md)
  - **Drilldown inline** abaixo do grid (micro): título "Segunda · Noturno", uma linha por sala ordenada por taxa desc — nome, minutos formatados ("3h 30min"), barra fina + badge %
  - Campus sem salas → empty state com link pra `/classrooms`
- `Web/app/pages/campi.vue` — botão ghost seta (`i-lucide-arrow-right`, tooltip "Ocupação", `:to="/campus/${id}"`) no card, espelhando a seta da tabela de classrooms
- Docs: seção "Ocupação" em `4.campus.md`

---

## Ordem e verificação

Ordem: **A → B → C** (A limpa DTOs; B grava `ClassroomId`; C lê).

```bash
dotnet test --filter "FullyQualifiedName~Campi"
dotnet test --filter "FullyQualifiedName~AssignClassroomsToClassSchedules"
dotnet test --filter "FullyQualifiedName~GetCampusOccupancy"
dotnet test --filter "FullyQualifiedName~IntegrationTests"   # varredura completa
```

- Nunca rodar `dotnet build`; migração (`dotnet ef migrations add RemoveCampusCapacity`) fica com o usuário
- Typecheck/build do frontend não roda neste WSL (Windows-only) — handoff pro usuário
