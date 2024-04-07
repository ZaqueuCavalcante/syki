# STRUCTURE

- Back
    - Controller
    - Service
    - Entity
    - DbConfig
    - Task?
    - Examples?
- Shared
    - Dtos
- Front
    - Client
    - Page
    - Component
- Daemon
    - TaskHandler
- Tests
    - Unit
    - Integration
    - BUnit?
    - E2E?
    - Mutation?

# DONE

- [X] 00 CreatePendingUserRegister
- [X] 01 FinishUserRegister
- [X] 02 CreateUser
- [X] 03 Login
- [X] 04 GenerateJWT
- [X] 05 GetMfaKey
- [X] 06 SetupMfa
- [X] 07 LoginMfa
- [X] 08 SendResetPasswordToken
- [X] 09 ResetPassword

- [X] 10 GetAcademicInsights
- [X] 11 CreateCampus
- [X] 12 UpdateCampus
- [X] 13 GetCampi
- [X] 14 CreateCurso
- [X] 15 GetCursos
- [X] 16 CreateDisciplina
- [X] 17 GetDisciplinas



# TODO

- [] GetAlunoInsights
- [] GetAdmInsights



- [] GetCursoDisciplinas
- [] GetCursosWithDisciplinas



- [] CreateGrade
- [] GetGrades
- [] GetGradeDisciplinas

- [] CreateOferta
- [] GetOfertas

- [] CreateProfessor
- [] GetProfessores

- [] CreateTurma
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

- Features:
    - User register
    - CRUDs
    - Flow...

- Stack:
    - C#, ASP.NET, Blazor Wasm, NGINX, PostgreSQL
    - Docker, Docker-Compose
    - Tests (unit, integration, E2E, Mutation...)
    - Libs
    - Documentation
    - Infra (Railway), CI/CD (GitHub Actions)

- Code:
    - Auditoria
    - Autenticacao / autorizacao
    - Autenticacao / autorizacao (roles)
    - RateLimit
    - Tarefas em background'
    - ER do banco de dados




## 00 CreatePendingUserRegister
    - Cadastrar email do academico da instituicao
    - Mostrar erros de email invalido e email duplicado

## 01 FinishUserRegister
    - 




