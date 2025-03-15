namespace Syki.Front.Features.Adm.GetBatch;

public class GetBatchClient(HttpClient http) : IAdmClient
{
    public async Task<BatchOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<BatchOut>($"/adm/batches/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
