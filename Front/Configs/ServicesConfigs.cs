

namespace Syki.Front.Configs;

public static class ServicesConfigs
{
    public static void AddServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        // Cross
        builder.Services.AddScoped<CreatePendingUserRegisterClient>();
        builder.Services.AddScoped<FinishUserRegisterClient>();
        builder.Services.AddScoped<LoginClient>();
        builder.Services.AddScoped<LoginMfaClient>();
        builder.Services.AddScoped<GetMfaKeyClient>();
        builder.Services.AddScoped<SetupMfaClient>();
        builder.Services.AddScoped<SendResetPasswordTokenClient>();
        builder.Services.AddScoped<ResetPasswordClient>();
        builder.Services.AddScoped<GetAcademicInsightsClient>();
        builder.Services.AddScoped<GetTeacherInsightsClient>();

        // Academic
        builder.Services.AddScoped<CreateCampusClient>();
        builder.Services.AddScoped<UpdateCampusClient>();
        builder.Services.AddScoped<GetCampiClient>();

        builder.Services.AddScoped<CreateGradeClient>();
        builder.Services.AddScoped<GetAdmInsightsClient>();

        builder.Services.AddScoped<CreateCourseClient>();
        builder.Services.AddScoped<GetCoursesClient>();
        builder.Services.AddScoped<GetDisciplinesClient>();

        builder.Services.AddScoped<CreateDisciplinaClient>();
        builder.Services.AddScoped<GetAlunoInsightsClient>();
        builder.Services.AddScoped<GetStudentAgendaClient>();
        builder.Services.AddScoped<GetStudentDisciplinesClient>();
        builder.Services.AddScoped<GetProfessorAgendaClient>();

        builder.Services.AddScoped<CreateAcademicPeriodClient>();
        builder.Services.AddScoped<GetAcademicPeriodsClient>();
        builder.Services.AddScoped<CreateEnrollmentPeriodClient>();
        builder.Services.AddScoped<GetEnrollmentPeriodsClient>();
        builder.Services.AddScoped<GetCurrentEnrollmentPeriodClient>();

        // Student

        // Teacher

        // Adm
    }
}
