using Exato.Shared.Features.Cross.GetUserAccount;

namespace Exato.Front.Features.Cross.GetUserAccount;

public class GetUserAccountClient(HttpClient http) : ICrossClient
{
    public async Task<GetUserAccountOut> Get()
    {
        return await http.GetFromJsonAsync<GetUserAccountOut>("user/account", HttpConfigs.JsonOptions) ?? new();
    }
}
