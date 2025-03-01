namespace Syki.Front.Features.Adm.GetCommandsSummary;

public class GetCommandsSummaryClient(HttpClient http) : IAdmClient
{
    public async Task<GetCommandsSummaryOut> Get()
    {
        return await http.GetFromJsonAsync<GetCommandsSummaryOut>("/adm/commands-summary", HttpConfigs.JsonOptions) ?? new();
    }
}
