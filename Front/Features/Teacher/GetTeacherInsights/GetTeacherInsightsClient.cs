namespace Syki.Front.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsClient(HttpClient http)
{
    public async Task<IndexProfessorOut> Get()
    {
        return await http.GetFromJsonAsync<IndexProfessorOut>("/professor-insights") ?? new();
    }
}
