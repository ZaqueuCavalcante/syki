using Exato.Front.Auth;
using Microsoft.JSInterop;
using Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;

namespace Exato.Front.Features.Cross.TwoFactorAuthenticationLogin;

public class TwoFactorAuthenticationLoginClient(HttpClient http, ILocalStorageService localStorage, ExatoAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<OneOf<TwoFactorAuthenticationLoginOut, ErrorOut>> Login(string code)
    {
        var body = new TwoFactorAuthenticationLoginIn { Token = code };
        var response = await http.PostAsJsonAsync("2fa/login", body);

        var result = await response.Resolve<TwoFactorAuthenticationLoginOut>();

        if (result.IsSuccess)
        {
            await localStorage.SetItemAsync("User", result.Success);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
