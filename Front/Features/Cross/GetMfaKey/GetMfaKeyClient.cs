namespace Syki.Front.Features.Cross.GetMfaKey;

public class GetMfaKeyClient(HttpClient http) : ICrossClient
{
    public async Task<GetMfaKeyOut> Get()
    {
        return await http.GetFromJsonAsync<GetMfaKeyOut>("/mfa/key", HttpConfigs.JsonOptions) ?? new();
    }
}
