namespace Syki.Front.Features.Adm.GetBatches;

public class GetBatchesClient(HttpClient http) : IAdmClient
{
    public async Task<List<CommandBatchTableOut>> Get(CommandBatchTableFilterIn filters)
    {
        return await http.GetFromJsonAsync<List<CommandBatchTableOut>>("/adm/batches".AddQueryString(filters), HttpConfigs.JsonOptions) ?? [];
    }
}
