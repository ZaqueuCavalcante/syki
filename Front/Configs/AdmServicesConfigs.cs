namespace Syki.Front.Configs;

public static class AdmServicesConfigs
{
    public static void AddAdmServicesConfigs(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddScoped<GetAdmInsightsClient>();
        builder.Services.AddScoped<GetInstitutionsClient>();
        builder.Services.AddScoped<GetUsersClient>();
    }
}
