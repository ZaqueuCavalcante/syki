namespace Syki.Front.GetAdmInsights;

public class GetAdmInsightsClient(HttpClient http)
{
    public async Task<IndexAdmOut> Get()
    {
        return await http.GetFromJsonAsync<IndexAdmOut>("/adm-insights") ?? new();
    }
}
