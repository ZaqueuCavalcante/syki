namespace Syki.Front.Features.Adm.GetCommands;

public class GetCommandsClient(HttpClient http) : IAdmClient
{
    public async Task<List<CommandTableOut>> Get(CommandTableFilterIn filters)
    {
        return await http.GetFromJsonAsync<List<CommandTableOut>>("/adm/commands".AddQueryString(filters), HttpConfigs.JsonOptions) ?? [];
    }
}
