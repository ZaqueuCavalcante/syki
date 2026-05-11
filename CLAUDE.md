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

Role-specific HTTP clients (`AcademicHttpClient`, `TeacherHttpClient`, etc.) wrap all API calls and live in `Tests/Clients/`. Integration tests run in parallel (`[Parallelizable(ParallelScope.All)]`).

For tests that trigger async command processing use:

```csharp
await _back.AwaitCommandsProcessing();
```
