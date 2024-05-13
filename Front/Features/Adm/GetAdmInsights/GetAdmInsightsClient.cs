namespace Syki.Front.Features.Academic.GetAdmInsights;

public class GetAdmInsightsClient(HttpClient http)
{
    public async Task<IndexAdmOut> Get()
    {
        return await http.GetFromJsonAsync<IndexAdmOut>("/adm/adm-insights") ?? new();
    }
}
