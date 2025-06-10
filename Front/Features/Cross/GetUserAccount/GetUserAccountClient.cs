namespace Syki.Front.Features.Cross.GetUserAccount;

public class GetUserAccountClient(HttpClient http) : ICrossClient
{
    public async Task<GetUserAccountOut> Get()
    {
        return await http.GetFromJsonAsync<GetUserAccountOut>("/user/account", HttpConfigs.JsonOptions) ?? new();
    }
}
