# Shortcuts de cenário nos testes de integração

## Contexto

Montar uma turma nos testes exige 5 chamadas encadeadas (professor → disciplina → vincular → período → turma), mais 3 pra matricular aluno (período de matrícula → liberar → criar/vincular aluno). Esse bloco está **copiado verbatim em 4 arquivos**, sob 4 nomes diferentes:

| Arquivo | Helper |
|---|---|
| `Features/Students/CreateClassActivityWork/` | `CreateClassActivityWithEnrolledStudent(director, studentEmail)` |
| `Features/Students/GetStudentClassActivities/` | `CreateClassWithTeacher(director)` |
| `Features/Teachers/CreateClassActivity/` | `CreateTeacherClass(teacherEmail)` |
| `Features/Teachers/CreateLessonAttendance/` | `CreateTeacherClassWithLessons(director, teacherEmail)` |

Três sintomas de que a duplicação já apodreceu:

1. **`CreateTeacherClassWithLessons` mente no nome** — o corpo é idêntico a `CreateTeacherClass`, não cria lesson nenhuma. As lessons nascem dentro do `CreateClass` (`CreateClassService.cs:42` → `@class.CreateLessons()`).
2. **`CreateClassWithTeacher` retorna `(int ClassId, string TeacherEmail)`** — tupla é o retorno querendo virar objeto: cada helper devolve um subconjunto diferente do cenário, e o teste precisa do email pra fazer `LoginAs` depois.
3. **O `private` é uma ficção.** Os 105 arquivos de teste são um único `partial class IntegrationTests`. Todos os 7 helpers "privados" convivem numa classe só — é por isso que precisaram de nomes distintos, senão colidiriam. E `StartClasses`, declarado em `GetTeacherCurrentClasses`, **já é chamado por `GetTeacherClass` e `GetStudentClass`**. Ou seja: já existe uma API compartilhada de facto, espalhada por arquivos de feature, sem casa e sem contrato.

Este plano dá uma casa a esses helpers: um objeto de cenário + um shortcut parametrizado, seguindo o precedente do `Base/BackFactory.Identity.cs` (`LoggedAsDirector` / `LoggedAsTeacher` / `LoginAs` já são exatamente isso — extensions em `BackFactory` encapsulando fluxo multi-step).

**Decisões:**
- Shortcut = **extension em `BackFactory`** com optional params (casa com o estilo do `CreateClass`, que já é assim), não builder fluente
- Retorno = **objeto `ClassScenario`**, não tupla — expõe ids, nomes e emails
- Setup vai **via HTTP**, chamando os próprios endpoints — mantém o shortcut honesto (não inventa estado que a API não produz). **Exceção: `started`** (ver abaixo)
- **Nada de `CreateFullClass()` monolítico** — os testes param em pontos diferentes do ciclo de vida (criada → liberada → iniciada); um método que faz tudo forçaria metade dos testes a montar estado a mais
- Testes de `CreateClass` e `StartClass` **ficam com arrange explícito** — teste do endpoint X não usa shortcut que chama o endpoint X, senão esconde o que está sob teste

---

## A regra que obriga `started` a escrever no banco

**Não existe caminho via API pra chegar em turma `Started` com alunos matriculados.** As regras se excluem:

| Serviço | Exige |
|---|---|
| `ReleaseClassForEnrollmentService` | período de matrícula **ativo** (`StartAt <= today <= EndAt`), senão `NoCurrentEnrollmentPeriod` |
| `AssignStudentToClassService` | `Status == OnEnrollment` |
| `StartClassService` | `Status == OnEnrollment` **e nenhum** período ativo, senão `EnrollmentPeriodMustBeFinalized` |

Liberar exige período ativo; iniciar exige período encerrado. Como nenhum endpoint encerra/edita o período, a sequência liberar → matricular → iniciar é inalcançável num teste só.

Por isso o `StartClasses` atual escreve `Status = Started` direto no `DbContext`. **Não é preguiça, é a única saída** — e o `started: true` do cenário deve absorver essa abordagem como está. Comentar isso no código, senão alguém "conserta" pro endpoint e quebra os testes.

---

## Fase A — Infra

### A1. `Tests/Base/ClassScenario.cs` (novo)

```csharp
using Estud.Tests.Integration.Clients;

namespace Estud.Tests.Base;

public class ClassScenario
{
    public required TestsHttpClient Director { get; init; }
    public int ClassId { get; init; }
    public int DisciplineId { get; init; }
    public string Discipline { get; init; } = "";
    public int PeriodId { get; init; }
    public int TeacherId { get; init; }
    public string TeacherName { get; init; } = "";
    public string TeacherEmail { get; init; } = "";
    public List<ClassScenarioStudent> Students { get; init; } = [];

    public ClassScenarioStudent Student => Students[0];   // conveniência p/ o caso de 1 aluno
}

public class ClassScenarioStudent
{
    public int Id { get; init; }
    public string Name { get; init; } = "";
    public string Email { get; init; } = "";
}
```

Nomes (`Discipline`, `TeacherName`, `Students[].Name`) são necessários porque os testes assertam neles — ex: `details.Teacher.Should().Be(teacherName)`, `details.Students[0].Name.Should().Be(studentName)`, `details.Discipline.Should().Be("Geometria")`.

`Estud.Tests.Base` já é global using (`GlobalUsings.cs`), então nenhum teste precisa de `using` novo.

### A2. `Tests/Base/BackFactory.Academic.cs` (novo)

```csharp
public static class BackFactoryAcademic
{
    public static async Task<ClassScenario> CreateClassScenario(
        this BackFactory factory,
        TestsHttpClient? director = null,   // reaproveita o director quando o teste já tem um
        string discipline = "Geometria",    // defaults iguais aos do TestsHttpClient
        string period = "2024.1",
        int students = 0,                   // > 0 implica liberar pra matrícula
        bool releasedForEnrollment = false,
        bool started = false
    )
```

Ordem:
1. `director ??= await factory.LoggedAsDirector()`
2. Professor: `DataGen.UserName` + `DataGen.Email` → `CreateTeacher`
3. `CreateDiscipline(discipline)` → `AssignDisciplinesToTeacher(teacher.Id, [discipline.Id])`
4. `CreateAcademicPeriod(period)`
5. `CreateClass(discipline.Id, period.Id, teacherId: teacher.Id)` (schedule default do client: seg 07:00–09:00)
6. Se `students > 0 || releasedForEnrollment || started`: `CreateEnrollmentPeriod(startAt: today.AddDays(-2), endAt: today.AddDays(2))` + `ReleaseClassForEnrollment(class.Id)`
7. `students` vezes: `CreateStudent(DataGen.UserName, DataGen.Email)` + `AssignStudentToClass`
8. Se `started`: escrita direta no banco (ver seção acima), com comentário explicando o porquê

Não expor `TeacherClient`/`StudentClient` prontos — custaria um login HTTP em todo teste que não precisa. O teste faz `await _back.LoginAs(scenario.TeacherEmail)`, uma linha e explícito.

### A3. Mover `StartClasses` pra `BackFactory.Academic.cs`

```csharp
public static async Task StartClasses(this BackFactory factory, params int[] ids)
```

Corpo idêntico ao atual (`GetTeacherCurrentClassesIntegrationTests.cs:125`). Já é usado por 3 arquivos — só está no lugar errado. Call sites viram `await _back.StartClasses(...)`.

### A4. Mover `GetFirstLessonId` pra `BackFactory.Academic.cs`

```csharp
public static async Task<int> GetFirstLessonId(this BackFactory factory, int classId)
```

Corpo idêntico (`CreateLessonAttendanceIntegrationTests.cs`). Consulta de leitura, mesma natureza dos helpers do `BackFactory.Identity.cs`.

---

## Fase B — Migrar os helpers duplicados

Deletar os 4 helpers e reescrever os call sites.

### B1. `Features/Students/CreateClassActivityWork/`
Deletar `CreateClassActivityWithEnrolledStudent` (18 linhas). Padrão novo:
```csharp
var scenario = await _back.CreateClassScenario(director, students: 1);
var teacherClient = await _back.LoginAs(scenario.TeacherEmail);
var activity = (await teacherClient.CreateClassActivity(scenario.ClassId)).Success;
var client = await _back.LoginAs(scenario.Student.Email);
```
Remover o `using Estud.Tests.Integration.Clients;` da linha 1 — só existia porque a assinatura do helper nomeava `TestsHttpClient`.

**Atenção ao teste `Should_not_create_work_when_student_enrolled_after_activity_creation`** (linha ~135): ele depende da ordem **atividade criada → aluno matriculado**, o inverso do que o cenário faz. Usar `CreateClassScenario(director, releasedForEnrollment: true)` (sem alunos), criar a atividade, e só então `CreateStudent` + `AssignStudentToClass` à mão.

### B2. `Features/Students/GetStudentClassActivities/`
Deletar `CreateClassWithTeacher` e `EnrollStudentInClass` → `CreateClassScenario(director, students: 1)`.

### B3. `Features/Teachers/CreateClassActivity/`
Deletar `CreateTeacherClass(teacherEmail)`. Ele criava o próprio director; agora `CreateClassScenario()` sem args resolve. O email do professor sai de `scenario.TeacherEmail` em vez de ser passado por fora.

### B4. `Features/Teachers/CreateLessonAttendance/`
Deletar `CreateTeacherClassWithLessons` e `EnrollStudentsInClass` → `CreateClassScenario(director, students: count)`; `scenario.Students.Select(s => s.Id)` cobre o retorno do antigo. `GetFirstLessonId` vira `_back.GetFirstLessonId(...)` (A4).

---

## Fase C — Arranges inline

Mesmo bloco, sem helper, direto no teste. Migrar só onde o cenário serve **inteiro**; onde o teste precisa de uma variação (turma sem professor, professor não vinculado, turma de outra instituição), **deixar explícito** — é justamente o desvio que está sob teste.

- `Features/Teachers/GetTeacherClass/` — happy paths migram; os 3 testes de erro (`TeacherNotAssignedToClass`, outra instituição) ficam explícitos, pois dependem de turma *sem* professor ou com *outro* professor
- `Features/Students/GetStudentClass/` — idem: happy path migra, erros ficam
- `Features/Teachers/GetTeacherClassActivities/`, `Features/Teachers/GetTeacherClassActivity/`
- `Features/Parents/GetParentStudentAgenda/`, `Features/Students/AssignStudentToClass/`
- `Features/Classes/GetClass/`, `Features/Classes/GetClasses/` — avaliar caso a caso; são testes do próprio módulo de turmas
- **Não tocar**: `Features/Classes/CreateClass/`, `Features/Classes/StartClass/`

Fase C é incremental — pode parar em qualquer ponto sem deixar o código inconsistente.

---

## Fora de escopo

- **`activities: int` no cenário.** 4 arquivos precisam de "turma + atividade", mas os params de atividade variam muito (`note`, `title`, `type`, `weight`) e os testes assertam neles; um `activities: 1` genérico só serviria pro caso "preciso que exista uma". Reavaliar depois da Fase C.
- **Cenários de curso/oferta/matrícula** (`EnrollStudentInCourseOffering`, `CourseCurriculums`) — outro eixo, mesmo tratamento se valer a pena depois.
- **Trocar setup HTTP por inserts diretos** pra ganhar velocidade. Seria outra discussão; o setup atual já é via HTTP e mudar isso agora misturaria dois objetivos.

## Risco conhecido

O shortcut monta o cenário chamando os próprios endpoints: se `CreateClass` quebrar, dezenas de testes não relacionados ficam vermelhos junto. Isso **já é verdade hoje** com os helpers privados — não é regressão introduzida aqui. É o motivo da decisão de manter `CreateClass`/`StartClass` com arrange explícito.

---

## Ordem e verificação

Ordem: **A → B → C**. A é aditiva (nada quebra); B e C podem ir arquivo por arquivo.

```bash
dotnet test --filter "FullyQualifiedName~CreateClassActivityWork"
dotnet test --filter "FullyQualifiedName~CreateLessonAttendance"
dotnet test --filter "FullyQualifiedName~GetStudentClassActivities"
dotnet test --filter "FullyQualifiedName~CreateClassActivity"
dotnet test --filter "FullyQualifiedName~IntegrationTests"   # varredura completa
```

A suíte é a própria verificação: refactor de teste sem mudança de produção, então **todo teste que passava tem que continuar passando** — nenhum assert muda, só o arrange. Ao final, `grep -rn "private async Task" Tests/Features/` deve voltar vazio (ou só com helpers genuinamente locais a uma feature).

- Nunca rodar `dotnet build`; os testes rodam no Windows (SDK .NET 10 ausente no WSL) — execução fica com o usuário
