# DONE

- [] Cadastro informando meu email
- [] Definir minha senha
- [] Fazer login
- [] Configurar o segundo fator de autenticacao
- [] Fazer login usando minha senha + token
- [] Insights sumario dos dados cadastrados (dados de teste)
- [] Campus
    - Criar Agreste II
    - Editar pra Agreste I
    - Buscar por nome e cidade
- [] Cursos
    - Criar bacharelado em medicina
    - Buscar medicina
- [] Disciplinas
    - Criar geometria analítica
    - Vincular com os cursos de exatas
- [] Grades
    - Mostrar a grade de ADS ja criada
    - Dos 10 cursos, apenas 4 possuem disciplinas vinculadas
    - Mostrar selecionando varios cursos
    - Criar grade de direito com 10 disciplinas
- [] Ofertas
    - Ofertar curso de direito matutino garoa
    - Dos 10 cursos, so mostra os que possuem grade
- [] Professores
    - Criar professor Reginaldo
        - reginaldo@syki.com
    - Acessar link, criar senha e logar
- [] Turmas
    - Criar todas essas em timelapse
    - Design de Interação Humano-Máquina (Joselia, 19-21)
    - Introdução à Redes de Computadores (Antonio, 19-22)
    - Introdução ao Desenvolvimento Web (Davi, 19-22)
    - Matemática Discreta (Reginaldo, 21-22, 19-22)
    - Pensamento Computacional e Algoritmos (Manu, 19-22)
    - Projeto Integrador I: Concepção e Prototipação (Reginaldo, 9-11)
    - Direito e Economia (Luciete, 830-12)
    - Introdução ao Direito (Paulo, 830-12)
- [] Alunos
    - Criar aluno Chico
        - chico@syki.com
- [] Periodos academicos
    - Criar o 2025.1 01/02 ate 05/06
- [] Matriculas 
- [] Notificacoes


- [] 
- [] 
- [] 
- [] 
- [] 




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
- [X] 18 CreateGrade

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
