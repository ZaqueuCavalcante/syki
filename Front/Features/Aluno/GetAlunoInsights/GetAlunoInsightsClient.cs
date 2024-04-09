namespace Syki.Front.GetAlunoInsights;

public class GetAlunoInsightsClient(HttpClient http)
{
    public async Task<IndexAlunoOut> Get()
    {
        return await http.GetFromJsonAsync<IndexAlunoOut>("/aluno-insights") ?? new();
    }
}
