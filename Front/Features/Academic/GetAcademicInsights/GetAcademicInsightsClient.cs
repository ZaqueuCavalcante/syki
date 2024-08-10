namespace Syki.Front.Features.Academic.GetAcademicInsights;

public class GetAcademicInsightsClient(HttpClient http) : IAcademicClient
{
    public async Task<AcademicInsightsOut> Get()
    {
        return await http.GetFromJsonAsync<AcademicInsightsOut>("/academic/insights") ?? new();
    }
}
