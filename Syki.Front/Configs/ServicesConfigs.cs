using Syki.Front.GetAlunoDisciplinas;

namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<LoginClient>();
        builder.Services.AddScoped<LoginMfaClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();
        builder.Services.AddScoped<SetupMfaClient>();
        builder.Services.AddScoped<SendResetPasswordTokenClient>();
        builder.Services.AddScoped<ResetPasswordClient>();
        builder.Services.AddScoped<GetAcademicInsightsClient>();
        builder.Services.AddScoped<GetProfessorInsightsClient>();

        builder.Services.AddScoped<CreateCampusClient>();
        builder.Services.AddScoped<UpdateCampusClient>();
        builder.Services.AddScoped<GetCampiClient>();

        builder.Services.AddScoped<CreateGradeClient>();
        builder.Services.AddScoped<GetAdmInsightsClient>();

        builder.Services.AddScoped<CreateCursoClient>();
        builder.Services.AddScoped<GetCursosClient>();
        builder.Services.AddScoped<GetDisciplinasClient>();

        builder.Services.AddScoped<CreateDisciplinaClient>();
        builder.Services.AddScoped<GetAlunoInsightsClient>();
        builder.Services.AddScoped<GetAlunoAgendaClient>();
        builder.Services.AddScoped<GetAlunoDisciplinasClient>();
        builder.Services.AddScoped<GetProfessorAgendaClient>();

        builder.Services.AddScoped<CreateAcademicPeriodClient>();
        builder.Services.AddScoped<GetAcademicPeriodsClient>();
        builder.Services.AddScoped<CreateEnrollmentPeriodClient>();
        builder.Services.AddScoped<GetEnrollmentPeriodsClient>();
        builder.Services.AddScoped<GetCurrentEnrollmentPeriodClient>();
    }
}
