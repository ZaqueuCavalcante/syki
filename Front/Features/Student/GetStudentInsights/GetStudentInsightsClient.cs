namespace Syki.Front.Features.Student.GetStudentInsights;

public class GetStudentInsightsClient(HttpClient http) : IStudentClient
{
    public async Task<StudentInsightsOut> Get()
    {
        return await http.GetFromJsonAsync<StudentInsightsOut>("/student/insights") ?? new();
    }
}
