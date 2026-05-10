using System.Net.Http.Json;

namespace Syki.Tests.Clients;

public class AdmHttpClient(HttpClient http)
{
    public readonly HttpClient Cross = http;

    public async Task<List<UserOut>> GetUsers()
    {
        return await Cross.GetFromJsonAsync<List<UserOut>>("/adm/users", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<List<InstitutionOut>> GetInstitutions()
    {
        return await Cross.GetFromJsonAsync<List<InstitutionOut>>("/adm/institutions", HttpConfigs.JsonOptions) ?? [];
    }

    public async Task<AdmInsightsOut> GetAdmInsights()
    {
        return await Cross.GetFromJsonAsync<AdmInsightsOut>("/adm/insights", HttpConfigs.JsonOptions) ?? new();
    }

    public async Task SetupFeatureFlags(bool crossLogin)
    {
        var data = new SetupFeatureFlagsIn { CrossLogin = crossLogin };
        await Cross.PutAsJsonAsync("/adm/feature-flags", data);
    }
}
