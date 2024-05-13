namespace Syki.Front.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsClient(HttpClient http)
{
    public async Task<IndexTeacherOut> Get()
    {
        return await http.GetFromJsonAsync<IndexTeacherOut>("/professor-insights") ?? new();
    }
}
