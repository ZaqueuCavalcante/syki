using Exato.Shared.Features.Cross.SetupTwoFactorAuthentication;

namespace Exato.Front.Features.Cross.SetupTwoFactorAuthentication;

public class SetupTwoFactorAuthenticationClient(HttpClient http) : ICrossClient
{
    public async Task<bool> Setup(string token)
    {
        var data = new SetupTwoFactorAuthenticationIn { Token = token };
        var response = await http.PostAsJsonAsync("2fa/setup", data);
        return response.IsSuccessStatusCode;
    }
}
