using Syki.Back.Features.Adm.GetUsers;
using Syki.Back.Features.Adm.GetAdmInsights;
using Syki.Back.Features.Adm.GetInstitutions;

namespace Syki.Back.Configs;

public static class AdmServicesConfigs
{
    public static void AddAdmServicesConfigs(this IServiceCollection services)
    {
        services.AddScoped<GetAdmInsightsService>();
        services.AddScoped<GetInstitutionsService>();
        services.AddScoped<GetUsersService>();
    }
}
