namespace Syki.Front.Features.Student.GetStudentInsights;

public class GetAlunoInsightsClient(HttpClient http)
{
    public async Task<IndexStudentOut> Get()
    {
        return await http.GetFromJsonAsync<IndexStudentOut>("/aluno-insights") ?? new();
    }
}
