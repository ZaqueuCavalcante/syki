namespace Syki.Front.Features.Adm.GetAdmInsights;

public class GetAdmInsightsClient(HttpClient http) : IAdmClient
{
    public async Task<AdmInsightsOut> Get()
    {
        return await http.GetFromJsonAsync<AdmInsightsOut>("/adm/insights", HttpConfigs.JsonOptions) ?? new();
    }
}
