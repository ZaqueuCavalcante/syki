- [] CreateAcademicPeriod
- [] CreateAluno
- [] CreateCampus
- [] CreateEnrollmentPeriod
- [] CreatePendingUserRegister
- [] CreateUser
- [] FinishUserRegister
- [] GenerateJWT
- [] GetAcademicPeriods
- [] GetCampi
- [] GetCurrentEnrollmentPeriod
- [] GetEnrollmentPeriods
- [] GetMfaKey
- [] Login
- [] LoginMfa
- [] LoginWithGoogle
- [] ResetPassword
- [] SendResetPasswordToken
- [] SetupMfa
- [] UpdateCampus
- [] GetAcademicInsights
- [] GetAlunoInsights
- [] GetAdmInsights
- [] UpdateCampus
- [] GetAlunoAgenda
- [] CreateCurso
- [] GetCursos
- [] CreateDisciplina
- [] GetDisciplinas
- [] CreateGrade
- [] GetCursoDisciplinas
- [] GetCursosWithDisciplinas
- [] GetGrades
- [] GetGradeDisciplinas
- [] CreateOferta
- [] GetOfertas
- [] CreateProfessor
- [] GetProfessores
- [] CreateTurma
- [] GetTurmas
- [] GetAlunos
- [] GetAlunoDisciplinas
- [] CreateNotification
- [] GetNotifications
- [] CreateInstitution
- [] GetInstitutions
- [] CreateMatriculaAluno
- [] GetMatriculaAlunoTurmas
- [] RegisterUser
- [] GetUsers


🗃️ Vertical Slice Architecture

Até o momento estava separando os arquivos do meu projeto em camadas, com uma pasta pra cada coisa: entidades de domínio, mapeamentos do EF, services, controllers...

Mas a medida que o sistema vai crescendo, não fica tão claro quais as funcionalidades já implementadas nem como elas se relacionam.

Assim refatorei todo o projeto, agora estou separando as coisas por feature (vertical slice): cada funcionalidade tem sua pasta, onde ficam as classes envolvidas na execução da feature.

Isso têm me ajudado a ser mais assertivo na definição de escopos, ter mais foco nas implementações e a pensar melhor nos casos de teste.

No GIF mostro como ficou a feature de criar um registro de usuário pendente no sistema.

Code: https://github.com/ZaqueuCavalcante/syki

# dotnet #softwarearchitecture #features #tests
