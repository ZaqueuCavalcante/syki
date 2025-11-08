using Exato.Shared.Features.Office.GetCommandBatches;

namespace Exato.Front.Features.Office.GetCommandBatches;

public class GetCommandBatchesClient(HttpClient http) : IOfficeClient
{
    public async Task<GetCommandBatchesOut> Get(GetCommandBatchesIn data)
    {
        return await http.GetFromJsonAsync<GetCommandBatchesOut>("office/command-batches".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
