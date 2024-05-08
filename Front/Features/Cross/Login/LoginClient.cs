using Newtonsoft.Json;
using Microsoft.JSInterop;

namespace Syki.Front.Features.Cross.Login;

public class LoginClient(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider)
{
    public async Task<LoginOut> Login(string email, string password)
    {
        var body = new LoginIn(email, password);
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
