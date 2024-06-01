namespace Syki.Front.Configs;

public static class StudentServicesConfigs
{
    public static void AddStudentServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<CreateStudentEnrollmentClient>();
        builder.Services.AddScoped<GetCurrentEnrollmentPeriodClient>();
        builder.Services.AddScoped<GetStudentAgendaClient>();
        builder.Services.AddScoped<GetStudentDisciplinesClient>();
        builder.Services.AddScoped<GetStudentEnrollmentClassesClient>();
        builder.Services.AddScoped<GetStudentInsightsClient>();
    }
}
