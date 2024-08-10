namespace Syki.Front.Features.Teacher.GetTeacherInsights;

public class GetTeacherInsightsClient(HttpClient http) : ITeacherClient
{
    public async Task<TeacherInsightsOut> Get()
    {
        return await http.GetFromJsonAsync<TeacherInsightsOut>("/teacher/insights") ?? new();
    }
}
