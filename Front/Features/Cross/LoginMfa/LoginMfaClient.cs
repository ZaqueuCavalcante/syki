using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace Syki.Front.Features.Cross.LoginMfa;

public class LoginMfaClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : ICrossClient
{
    public async Task<LoginMfaOut> Login(string code)
    {
        var body = new LoginMfaIn { Token = code };
        var response = await http.PostAsJsonAsync("/login/mfa", body);

        var responseAsString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginMfaOut>(responseAsString)!;

        if (response.IsSuccessStatusCode && result.AccessToken != null)
        {
            await localStorage.SetItemAsync("AccessToken", result.AccessToken);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
