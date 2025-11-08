using Exato.Shared.Features.Office.GetCommands;

namespace Exato.Front.Features.Office.GetCommands;

public class GetCommandsClient(HttpClient http) : IOfficeClient
{
    public async Task<GetCommandsOut> Get(GetCommandsIn data)
    {
        return await http.GetFromJsonAsync<GetCommandsOut>("office/commands".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
