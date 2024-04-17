# DONE

- [X] 00 CreatePendingUserRegister
- [X] 01 FinishUserRegister
- [] 02 CreateUser
- [] 03 Login
- [] 04 GenerateJWT
- [] 05 GetMfaKey
- [] 06 SetupMfa
- [] 07 LoginMfa
- [] 08 SendResetPasswordToken
- [] 09 ResetPassword

- [] 10 GetAcademicInsights
- [] 11 CreateCampus
- [] 12 UpdateCampus
- [] 13 GetCampi
- [] 14 CreateCurso
- [] 15 GetCursos
- [] 16 CreateDisciplina
- [] 17 GetDisciplinas
- [] 18 CreateGrade

- [] GetGrades
- [] GetGradeDisciplinas




- [] ?? CreateTurma

# TODO

- [] GetAlunoInsights
- [] GetAdmInsights

- [] GetCursoDisciplinas
- [] GetCursosWithDisciplinas

- [] CreateOferta
- [] GetOfertas

- [] CreateProfessor
- [] GetProfessores

- [] GetTurmas

- [] CreateAluno
- [] GetAlunos

- [] CreateAcademicPeriod
- [] GetAcademicPeriods
- [] CreateEnrollmentPeriod
- [] GetEnrollmentPeriods
- [] GetCurrentEnrollmentPeriod

- [] CreateNotification
- [] GetNotifications

- [] GetAlunoAgenda
- [] GetAlunoDisciplinas
- [] CreateMatriculaAluno
- [] GetMatriculaAlunoTurmas

- [] GetInstitutions
- [] GetUsers



# IDEAS

- N colocar o JWT no local storage
- Parar de fazer polling no banco (usar events?)
- Adicionar paginacao nos GETs
- Redis?
- RabbitMQ?
- Load Balancer?
- Monitoring? DataDog
- Optimize Queries
- Logs
- Use Result Pattern instead throw exceptions
- Load testing (K6)

# YouTube

- Thumb: imagem com todas as tecnologia
- Minutagem na descricao

- Features:
    - Cross, Academico, Professor, Aluno, Adm
    - Mostrar todos os erros possíveis?
    - 00 CreatePending user register
    - 01 FinishUserRegister
    - 02 CreateUser
    - 03 Login
    - 04 GenerateJWT
    - 05 GetMfaKey
    - 06 SetupMfa
    - 07 LoginMfa
    - 08 SendResetPasswordToken
    - 09 ResetPassword

    - 10 GetAcademicInsights
    - 11 CreateCampus
    - 12 UpdateCampus
    - 13 GetCampi
    - 14 CreateCurso
    - 15 GetCursos
    - 16 CreateDisciplina
    - 17 GetDisciplinas
    - 18 CreateGrade




- Arquitetura:
    - Back
    - Front (ngnix)
    - Daemon
    - Banco
    - *Tests

- Stack:
    - Back:
        - C#
        - ASP.NET
        - EF Core
        - Identity
        - Docs (Swagger + Redoc)
    - Front:
        - Blazor WebAssembly
        - MudBlazor
        - NGINX
    - Daemon:
        - C# Hosted App
    - Tests:
        - NUnit
        - Fluent Assertions
        - BUnit
        - Playwright
        - Stryker.NET
    - Banco:
        - PostgreSQL
    - Deploy:
        - CI/CD GitHub Actions
        - Docker
        - Docker-Compose
        - Railway

- Code:
    - Cada pasta é um projeto
    - Dockerfiles
    - Docker-Compose
    - Vertical Slices (todos os projetos)
    - Back:
        - Startup
            - ConfigureServices (Configs)
            - Configure (Middlewares)
        - Variáveis de ambiente
        - Features
        - Database (modelo ER)
        - Auditoria
        - Autenticacao / autorizacao
        - RateLimit
        - Exceptions
    - Front:
        - MudBlazor based
        - Clients (uso nos testes)
        - JWT no local storage?
        - Autenticacao / autorizacao
    - Daemon:
        - Pooling no banco
        - Processamento de tasks
    - Shared:
        - Código utilizado no back e no front
        - Tipicamente DTOs da API
    - Tests:
        - Unit
        - WebFactory, Base, Extensions, Integration
        - Components
        - E2E
        - Mutation
