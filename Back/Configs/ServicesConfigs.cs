using Syki.Back.Features.Academico.GetCampi;
using Syki.Back.Features.Academico.GetCursos;
using Syki.Back.Features.Academico.CreateCurso;
using Syki.Back.Features.Academico.CreateGrade;
using Syki.Back.Features.Academico.UpdateCampus;
using Syki.Back.Features.Academico.CreateCampus;
using Syki.Back.Features.Academico.CreateDisciplina;
using Syki.Back.Features.Academico.GetAcademicInsights;
using Syki.Back.CreateAluno;
using Syki.Back.GetEnrollmentPeriods;
using Syki.Back.CreateAcademicPeriod;
using Syki.Back.CreateProfessor;
using Syki.Back.CreateEnrollmentPeriod;
using Syki.Back.CreateMatriculaAluno;
using Syki.Back.CreateNotification;
using Syki.Back.CreateOferta;
using Syki.Back.CreateTurma;
using Syki.Back.GetAcademicPeriods;
using Syki.Back.GetAlunoAgenda;
using Syki.Back.GetAlunoDisciplinas;
using Syki.Back.GetAlunoInsights;
using Syki.Back.GetAlunos;
using Syki.Back.GetCurrentEnrollmentPeriod;
using Syki.Back.GetCursoDisciplinas;
using Syki.Back.GetCursosWithDisciplinas;
using Syki.Back.GetDisciplinas;
using Syki.Back.GetGradeDisciplinas;
using Syki.Back.GetGrades;
using Syki.Back.GetInstitutions;
using Syki.Back.GetMatriculaAlunoTurmas;
using Syki.Back.GetNotifications;
using Syki.Back.GetOfertas;
using Syki.Back.GetProfessores;
using Syki.Back.GetAdmInsights;
using Syki.Back.GetTurmas;
using Syki.Back.GetUsers;
using Syki.Back.GetCursosWithGrades;
using Syki.Back.GetProfessorInsights;
using Syki.Back.GetProfessorAgenda;
using Syki.Back.GetProfessorTurmas;
using Syki.Back.GetProfessorTurma;
using Syki.Back.Features.Cross.Login;
using Syki.Back.Features.Cross.SetupMfa;
using Syki.Back.Features.Cross.LoginMfa;
using Syki.Back.Features.Cross.GetMfaKey;
using Syki.Back.Features.Cross.GenerateJWT;
using Syki.Back.Features.Cross.ResetPassword;
using Syki.Back.Features.Cross.ViewNotification;
using Syki.Back.Features.Cross.FinishUserRegister;
using Syki.Back.Features.Cross.GetUserNotifications;
using Syki.Back.Features.Cross.SendResetPasswordToken;
using Syki.Back.Features.Cross.CreatePendingUserRegister;

namespace Syki.Back.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this IServiceCollection services)
    {
        // Cross
        services.AddScoped<CreatePendingUserRegisterService>();
        services.AddScoped<CreateUserService>();
        services.AddScoped<FinishUserRegisterService>();
        services.AddScoped<GenerateJWTService>();
        services.AddScoped<GetMfaKeyService>();
        services.AddScoped<LoginService>();
        services.AddScoped<LoginMfaService>();
        services.AddScoped<SetupMfaService>();
        services.AddScoped<ResetPasswordService>();
        services.AddScoped<SendResetPasswordEmailService>();
        services.AddScoped<ViewNotificationService>();
        services.AddScoped<GetUserNotificationsService>();

        // Academic
        services.AddScoped<CreateCampusService>();
        services.AddScoped<CreateCursoService>();
        services.AddScoped<GetAcademicInsightsService>();
        services.AddScoped<GetCampiService>();
        services.AddScoped<GetCursosService>();
        services.AddScoped<UpdateCampusService>();
        services.AddScoped<CreateDisciplinaService>();

        // Teacher
        services.AddScoped<CreateAcademicPeriodService>();
        services.AddScoped<CreateAlunoService>();
        services.AddScoped<CreateEnrollmentPeriodService>();
        services.AddScoped<CreateGradeService>();
        services.AddScoped<CreateMatriculaAlunoService>();
        services.AddScoped<CreateNotificationService>();
        services.AddScoped<CreateOfertaService>();
        services.AddScoped<CreateProfessorService>();
        services.AddScoped<CreateTurmaService>();

        // Student
        services.AddScoped<GetAcademicPeriodsService>();
        services.AddScoped<GetAlunoAgendaService>();
        services.AddScoped<GetAlunoDisciplinasService>();
        services.AddScoped<GetAlunoInsightsService>();
        services.AddScoped<GetAlunosService>();
        services.AddScoped<GetCurrentEnrollmentPeriodService>();
        services.AddScoped<GetCursoDisciplinasService>();
        services.AddScoped<GetCursosWithDisciplinasService>();
        services.AddScoped<GetCursosWithGradesService>();
        services.AddScoped<GetDisciplinasService>();
        services.AddScoped<GetEnrollmentPeriodsService>();
        services.AddScoped<GetGradeDisciplinasService>();
        services.AddScoped<GetGradesService>();
        services.AddScoped<GetInstitutionsService>();
        services.AddScoped<GetMatriculaAlunoTurmasService>();
        services.AddScoped<GetNotificationsService>();
        services.AddScoped<GetOfertasService>();
        services.AddScoped<GetProfessoresService>();
        services.AddScoped<GetAdmInsightsService>();
        services.AddScoped<GetTurmasService>();
        services.AddScoped<GetUsersService>();

        // Adm
        services.AddScoped<GetProfessorInsightsService>();
        services.AddScoped<GetProfessorAgendaService>();
        services.AddScoped<GetProfessorTurmasService>();
        services.AddScoped<GetProfessorTurmaService>();
    }
}
