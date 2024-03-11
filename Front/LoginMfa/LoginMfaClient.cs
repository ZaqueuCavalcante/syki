using Newtonsoft.Json;
using Syki.Front.Auth;
using Microsoft.JSInterop;

namespace Syki.Front.LoginMfa;

public class LoginMfaClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider)
{
    public async Task<LoginMfaOut> Login(string code)
    {
        var body = new LoginMfaIn { Code = code };
        var response = await http.PostAsJsonAsync("/login/mfa", body);

        var responseAsString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginMfaOut>(responseAsString)!;

        if (response.IsSuccessStatusCode)
        {
            await localStorage.SetItemAsync("AccessToken", result.AccessToken);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
