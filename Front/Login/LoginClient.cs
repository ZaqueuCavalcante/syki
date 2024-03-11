using Newtonsoft.Json;
using Syki.Front.Auth;
using Microsoft.JSInterop;

namespace Syki.Front.Login;

public class LoginClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider)
{
    public async Task<LoginOut> Login(string email, string password)
    {
        var body = new LoginIn { Email = email, Password = password };
        var response = await http.PostAsJsonAsync("/login", body);

        var responseAsString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginOut>(responseAsString)!;

        if (result.AccessToken != null)
        {
            await localStorage.SetItemAsync("AccessToken", result.AccessToken);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }
}
