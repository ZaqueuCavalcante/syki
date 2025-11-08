using Exato.Shared.Features.Cross.GetTwoFactorAuthenticationKey;

namespace Exato.Front.Features.Cross.GetTwoFactorAuthenticationKey;

public class GetTwoFactorAuthenticationKeyClient(HttpClient http) : ICrossClient
{
    public async Task<GetTwoFactorAuthenticationKeyOut> Get()
    {
        return await http.GetFromJsonAsync<GetTwoFactorAuthenticationKeyOut>("2fa/key", HttpConfigs.JsonOptions) ?? new();
    }
}
