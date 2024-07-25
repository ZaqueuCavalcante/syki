namespace Syki.Front.Configs;

public static class TeacherServicesConfigs
{
    public static void AddTeacherServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<GetTeacherClassClient>();
        builder.Services.AddScoped<GetTeacherAgendaClient>();
        builder.Services.AddScoped<GetTeacherClassesClient>();
        builder.Services.AddScoped<GetTeacherInsightsClient>();
    }
}
