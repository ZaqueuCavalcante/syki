namespace Syki.Front.GetProfessorInsights;

public class GetProfessorInsightsClient(HttpClient http)
{
    public async Task<IndexProfessorOut> Get()
    {
        return await http.GetFromJsonAsync<IndexProfessorOut>("/professor-insights") ?? new();
    }
}
