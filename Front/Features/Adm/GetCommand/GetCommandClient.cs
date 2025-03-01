namespace Syki.Front.Features.Adm.GetCommand;

public class GetCommandClient(HttpClient http) : IAdmClient
{
    public async Task<CommandOut> Get(Guid id)
    {
        return await http.GetFromJsonAsync<CommandOut>($"/adm/commands/{id}", HttpConfigs.JsonOptions) ?? new();
    }
}
