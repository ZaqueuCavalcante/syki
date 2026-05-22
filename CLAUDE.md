# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**Syki** is an open-source academic management system (SGA) for educational institutions.

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

Services return `OneOf<TOut, SykiError>`. Controllers use:

```csharp
var result = await service.Create(data);
return result.Match<IActionResult>(Ok, BadRequest);
```

Errors are singletons defined in `Back/Errors/SykiInvalidErrors.cs` and `SykiNotFoundErrors.cs`:

```csharp
public class InvalidCourseName : SykiError
{
    public static readonly InvalidCourseName I = new();
    public override string Code { get; set; } = nameof(InvalidCourseName);
    public override string Message { get; set; } = "Nome de curso inválido.";
}
```

Validators are nested private classes inside the Service and run via `V.Run(data, out var error)`.

### DbContext Conventions

`SykiDbContext` carries the current `InstitutionId` and `UserId` populated per-request (via middleware). Use these directly in services for multi-tenant scoping — never pass institution/user IDs manually through the call chain.

EF is configured with snake_case naming (`UseSnakeCaseNamingConvention`) and the `syki` schema.

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
public class CreateRoleService(SykiDbContext ctx) : ISykiService
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

    public async Task<OneOf<CreateRoleOut, SykiError>> Create(CreateRoleIn data)
    {
        if (V.Run(data, out var error)) return error;   // validation first

        var orgId = ctx.RequestUser.InstitutionId;      // multi-tenant scoping from ctx
        var exists = await ctx.Roles.AnyAsync(x => x.OwnerId == orgId && x.NormalizedName == ...);
        if (exists) return RoleNameAlreadyExists.I;     // domain checks after validation

        var role = new SykiRole(orgId, data.Name, data.Description, data.Permissions);
        ctx.Add(role);
        await ctx.SaveChangesAsync();

        return new CreateRoleOut { Id = role.Id };
    }
}
```

- `ISykiService` marker interface on every service.
- `Validator` is always a **private nested class**; static singleton `V`.
- Validation runs first via `V.Run(data, out var error)`.
- Institution/user context always comes from `ctx.RequestUser` — never from method parameters.
- Return `OneOf<TOut, SykiError>`; early-return errors as singletons (`.I`).

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
