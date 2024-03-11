namespace Syki.Front.GetAcademicInsights;

public class GetAcademicInsightsClient(HttpClient http)
{
    public async Task<AcademicInsightsOut> Get()
    {
        return await http.GetFromJsonAsync<AcademicInsightsOut>("/academic-insights") ?? new();
    }
}
