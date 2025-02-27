namespace Syki.Front.Features.Adm.GetFeatureFlags;

public class GetFeatureFlagsClient(HttpClient http) : IAdmClient
{
    public async Task<GetFeatureFlagsOut> Get()
    {
        return await http.GetFromJsonAsync<GetFeatureFlagsOut>("/adm/feature-flags", HttpConfigs.JsonOptions) ?? new();
    }
}
