# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Important constraints

- **Never run `dotnet build`** — the user builds itself.

## Backend conventions

### Checagem de strings — usar `HasValue()` / `IsEmpty()`

Em `if`s que checam strings, **sempre** usar as extensions `HasValue()` e `IsEmpty()` (definidas em `Back/Shared/Extensions/StringExtensions.cs`). **Nunca** usar `string.IsNullOrEmpty`, `string.IsNullOrWhiteSpace` nem suas negações.

**Correto:**
```csharp
if (name.HasValue()) { ... }
if (name.IsEmpty()) { ... }
```

**Errado:**
```csharp
if (!string.IsNullOrEmpty(name)) { ... }
if (string.IsNullOrWhiteSpace(name)) { ... }
```

### Comentários XML (`<summary>` / `<remarks>`) — sempre multi-linha

Nos comentários XML dos controllers, **sempre** colocar as tags de abertura e fechamento em linhas próprias, com o conteúdo numa linha separada. **Nunca** colocar o conteúdo na mesma linha da tag.

**Correto:**
```csharp
/// <summary>
/// Matricular aluno em oferta de curso
/// </summary>
/// <remarks>
/// Vincula um aluno a uma oferta de curso, criando uma matrícula.
/// </remarks>
```

**Errado:**
```csharp
/// <summary>Matricular aluno em oferta de curso</summary>
/// <remarks>Vincula um aluno a uma oferta de curso, criando uma matrícula.</remarks>
```

## Frontend conventions

### Zod validation — campos opcionais/undefined

Campos de formulário que podem ficar `undefined` (ex: selects não preenchidos) exigem `required_error` em português. Usar apenas `.min(1, '...')` não cobre o caso `undefined` e resulta na mensagem padrão do Zod em inglês ("Invalid input: expected string, received undefined").

**Correto (Zod v4):**
```ts
z.string({ error: 'Campo obrigatório' }).min(1, 'Campo obrigatório')
```

**Errado:**
```ts
z.string().min(1, 'Campo obrigatório')          // não cobre undefined
z.string({ required_error: '...' })             // API do Zod v3, não funciona no v4
```

### Event handlers (`@click` e similares) — sempre arrow function

Handlers de evento no template **sempre** usam arrow function com corpo em bloco. **Nunca** usar inline statement (a expressão solta, direto no atributo).

Vale para todos os handlers (`@click`, `@change`, `@submit`, `@keydown`, ...), inclusive quando o corpo é uma única atribuição.

**Correto:**
```vue
<UButton @click="() => { open = false }" />
<UButton @click="() => { open = false; selected = null }" />
<UInput @change="(e) => { search = e.target.value }" />
```

**Errado:**
```vue
<UButton @click="open = false" />
<UButton @click="open = false; selected = null" />
<UInput @change="search = $event.target.value" />
```

Chamada direta de um método já existente continua permitida: `@click="close"` ou `@click="() => { close() }"`.

### Tooltip em botão que abre modal/slideover

Botão com `UTooltip` que abre um modal/slideover ao clicar mantém o foco depois que o overlay fecha, deixando o tooltip preso na tela. Sempre dar `blur()` no elemento no `@click`:

```vue
<UTooltip text="Notificações">
  <UButton @click="(e) => { (e.currentTarget as HTMLElement).blur(); isOpen = true }" />
</UTooltip>
```

## Project Overview

**Estud** is an open-source academic management system (SGA) for educational institutions.

## Tech Stack

- **Backend**: ASP.NET Core (C#), PostgreSQL, EF Core + Dapper, Quartz.NET, SignalR, HybridCache, OpenTelemetry, Serilog, Scalar (API docs)
- **Frontend**: Nuxt.js (Vue 3 / TypeScript) with Nuxt UI — located in `Web/`
- **Tests**: NUnit + FluentAssertions, `WebApplicationFactory`-based integration tests against a real local PostgreSQL DB
- **Infra**: Docker, Railway, GitHub Actions CI/CD

## Commands

### Backend

```bash
# Run the full stack
docker-compose up
docker-compose build --no-cache   # rebuild after code changes

# Migrations
dotnet ef migrations add <MigrationName>          # create new migration
dotnet ef migrations script -o all_migrations.sql  # generate full SQL script
dotnet ef migrations script <FromMigration> <output>.sql  # single migration SQL
```

### Tests

```bash
# All tests
dotnet test --logger:"console;verbosity=detailed"

# Unit tests only
dotnet test --filter "FullyQualifiedName~UnitTests"

# Integration tests only
dotnet test --filter "FullyQualifiedName~IntegrationTests"

# Single test (by name substring)
dotnet test --filter "FullyQualifiedName~Should_create_course"

# Code coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Frontend

```bash
cd Web
pnpm install
pnpm dev
```

## Architecture

### Vertical Slice Architecture

All backend features live in `Back/Features/`, organized by user role:

```
Back/Features/
  Academic/   Teacher/   Student/   Adm/   Cross/   Identity/   Users/
```

Each feature is a self-contained folder (e.g. `CreateCourse/`) containing:

| File | Purpose |
|---|---|
| `CreateCourseController.cs` | HTTP endpoint, auth attribute, rate limiter, Swagger examples |
| `CreateCourseService.cs` | Business logic, FluentValidation, Result Pattern |
| `CreateCourseMapper.cs` | Entity → DTO extension methods |
| `Course.cs` | Domain entity |
| `CourseConfig.cs` | EF Core `IEntityTypeConfiguration<T>` |
| `CreateCourseIn/Out.cs` | DTOs (may live inline or in separate files) |

Test files mirror the same folder structure under `Tests/Features/`.

### Result Pattern

Services return `OneOf<TOut, EstudError>`. Controllers use:

```csharp
var result = await service.Create(data);
return result.Match<IActionResult>(Ok, BadRequest);
```

Errors are singletons defined in `Back/Errors/EstudInvalidErrors.cs` and `EstudNotFoundErrors.cs`:

```csharp
public class InvalidCourseName : EstudError
{
    public static readonly InvalidCourseName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseName);
    public override string Message { get; set; } = "Nome de curso inválido.";
}
```

Validators are nested private classes inside the Service and run via `V.Run(data, out var error)`.

### DbContext Conventions

`EstudDbContext` carries the current `InstitutionId` and `UserId` populated per-request (via middleware). Use these directly in services for multi-tenant scoping — never pass institution/user IDs manually through the call chain.

EF is configured with snake_case naming (`UseSnakeCaseNamingConvention`) and the `estud` schema.

### Async Command Processing

Business flows that are naturally asynchronous (emails, notifications, webhooks) use the `Command`/`CommandBatch` system:

1. A service creates a `Command` via `ctx.AddCommand(...)` — persisted in the `commands` table.
2. A Quartz.NET job (`CommandsProcessorJob`) picks up pending commands and dispatches them to handlers.
3. Commands support: parent-child relationships, batches, retry with exponential backoff, delayed execution (`NotBefore`).

### API Documentation

Every controller action uses XML summary comments + `SwaggerResponseExample`/`ErrorExamplesProvider` for Scalar docs. Input/output DTOs implement `IApiDto<T>` to provide named examples. Error types are passed as generic type parameters to `ErrorExamplesProvider<T1, T2, ...>`.

### POST Endpoint Pattern

Every POST feature follows the same 4-file structure. Below is the `CreateRole` feature as the canonical reference.

#### `CreateRoleController.cs` — HTTP layer only

```csharp
[ApiController, Authorize(Policies.CreateRole)]        // policy matches feature name
public class CreateRoleController(CreateRoleService service) : ControllerBase
{
    /// <summary>Criar perfil de acesso</summary>
    /// <remarks>Cria um novo perfil de acesso vinculado à organização do usuário logado.</remarks>
    [HttpPost("identity/roles")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateRoleIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateRoleIn>;   // declared even if unused
internal class ResponseExamples : ExamplesProvider<CreateRoleOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidRoleName,
    InvalidRoleDescription,
    InvalidPermissionsList,
    RoleNameAlreadyExists,
    InvalidRolePermissions
>;
```

- Controller holds **no logic** — just wires HTTP to the service.
- `Authorize(Policies.XYZ)` attribute name always matches the feature.
- XML `<summary>` (short) + `<remarks>` (full description) required on every action.
- `result.Match<IActionResult>(Ok, BadRequest)` is the only allowed result unwrapping.
- All possible error types are listed in `ErrorExamplesProvider<...>`.

#### `CreateRoleService.cs` — business logic

```csharp
public class CreateRoleService(EstudDbContext ctx) : IEstudService
{
    private class Validator : AbstractValidator<CreateRoleIn>
    {
        public Validator()
        {
            RuleFor(x => x.Name).NotEmpty().WithError(InvalidRoleName.I);
            RuleFor(x => x.Name).MaximumLength(50).WithError(InvalidRoleName.I);
            // ... more rules
        }
    }
    private static readonly Validator V = new();

    public async Task<OneOf<CreateRoleOut, EstudError>> Create(CreateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;   // validation first

        var orgId = ctx.RequestUser.InstitutionId;      // multi-tenant scoping from ctx
        var exists = await ctx.Roles.AnyAsync(x => x.OwnerId == orgId && x.NormalizedName == ...);
        if (exists) return RoleNameAlreadyExists.I;     // domain checks after validation

        var role = new EstudRole(orgId, data.Name, data.Description, data.Permissions);
        ctx.Add(role);
        await ctx.SaveChangesAsync();

        return new CreateRoleOut { Id = role.Id };
    }
}
```

- `IEstudService` marker interface on every service.
- `Validator` is always a **private nested class**; static singleton `V`.
- Validation runs first via `V.Run(data, out var error)`.
- Institution/user context always comes from `ctx.RequestUser` — never from method parameters.
- Return `OneOf<TOut, EstudError>`; early-return errors as singletons (`.I`).

#### `CreateRoleIn.cs` — input DTO

```csharp
public class CreateRoleIn : IApiDto<CreateRoleIn>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<int> Permissions { get; set; } = [];

    public static IEnumerable<(string, CreateRoleIn)> GetExamples() =>
    [
        ("Exemplo", new CreateRoleIn { Name = "Admin", Description = "...", Permissions = [1, 2, 3] }),
    ];
}
```

- Implements `IApiDto<T>` and provides at least one named example in `GetExamples()`.

#### `CreateRoleOut.cs` — output DTO

```csharp
public class CreateRoleOut : IApiDto<CreateRoleOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, CreateRoleOut)> GetExamples() =>
    [
        ("Exemplo", new CreateRoleOut { Id = 1 }),
    ];
}
```

- Same `IApiDto<T>` contract; `GetExamples()` drives the Scalar response example.

## Integration Tests

Tests use `WebApplicationFactory<Program>` (`BackFactory`). `IntegrationTestBase.OneTimeSetUp` drops and recreates the local test DB before each test class runs.

All integration tests are partial classes of `IntegrationTests`:

```csharp
public partial class IntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Should_create_course()
    {
        var client = await _back.LoggedAsAcademic();
        CreateCourseOut course = await client.CreateCourse("ADS", CourseType.Tecnologo, []);
        course.Id.Should().NotBeEmpty();
    }
}
```

For tests that trigger async command processing use:

```csharp
await _back.AwaitCommandsProcessing();
```

### Test file structure — `#region` grouping

Each feature's integration test file groups its tests into `#region` blocks, in this fixed order:

```csharp
public partial class IntegrationTests
{
    #region Authentication
    // unauthenticated request → 401 Unauthorized
    #endregion

    #region Authorization
    // authenticated but missing permission → 403 Forbidden
    #endregion

    #region Validation errors
    // invalid input or domain errors → ShouldBeError(SomeError.I)
    #endregion

    #region Happy path
    // valid request → asserts on result.Success
    #endregion
}
```

- **Not every feature needs every region.** Only include the regions that apply to the feature. For example, a `GET` endpoint with no input parameters (e.g. `GetRoles`, `GetDisciplines`) has nothing to validate, so it omits the **Validation errors** region entirely.
- Keep the regions that are present in the order above.
- Test method names follow `{Feature}_{Endpoint}_Should_{...}` (e.g. `Disciplines_GetDisciplines_Should_not_get_disciplines_when_not_authenticated`).
- Auth/authorization/error assertions use `result.ShouldBeError(...)`; happy-path assertions read the value via `result.Success`. This requires the corresponding `TestsHttpClient` method to return `OneOf<TOut, ErrorOut>` (via `response.Resolve<TOut>()`), not the raw DTO.
