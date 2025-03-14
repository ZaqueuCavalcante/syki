namespace Syki.Front.Features.Adm.GetBatchesSummary;

public class GetBatchesSummaryClient(HttpClient http) : IAdmClient
{
    public async Task<GetBatchesSummaryOut> Get()
    {
        return await http.GetFromJsonAsync<GetBatchesSummaryOut>("/adm/batches/summary", HttpConfigs.JsonOptions) ?? new();
    }
}
