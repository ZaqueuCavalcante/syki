namespace Syki.Front.Features.Adm.GetTasksSummary;

public class GetTasksSummaryClient(HttpClient http) : IAdmClient
{
    public async Task<GetTasksSummaryOut> Get()
    {
        return await http.GetFromJsonAsync<GetTasksSummaryOut>("/adm/tasks-summary") ?? new();
    }
}
