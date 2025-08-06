using Microsoft.JSInterop;

namespace Syki.Front.Features.Cross.LoginMfa;

public class LoginMfaClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<OneOf<LoginMfaOut, ErrorOut>> LoginMfa(string code)
    {
        var body = new LoginMfaIn { Token = code };
        var response = await http.PostAsJsonAsync("/login/mfa", body);

        var result = await response.Resolve<LoginMfaOut>();

        if (result.IsSuccess())
        {
            await localStorage.SetItemAsync("User", result.GetSuccess());
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
