using Exato.Shared.Features.Office.GetCommandBatchCommands;

namespace Exato.Front.Features.Office.GetCommandBatchCommands;

public class GetCommandBatchCommandsClient(HttpClient http) : IOfficeClient
{
    public async Task<GetCommandBatchCommandsOut> Get(Guid id, GetCommandBatchCommandsIn data)
    {
        return await http.GetFromJsonAsync<GetCommandBatchCommandsOut>($"office/command-batches/{id}/commands".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
