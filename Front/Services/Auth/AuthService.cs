using Syki.Front.Auth;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Security.Claims;

namespace Syki.Front.Services;

public class AuthService(HttpClient http, ILocalStorageService localStorage, SykiAuthStateProvider authStateProvider) : IAuthService
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

    public async Task<LoginMfaOut> LoginMfa(string code)
    {
        var body = new LoginMfaIn { Code = code };
        var response = await http.PostAsJsonAsync("/mfa/login", body);

        var responseAsString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginMfaOut>(responseAsString)!;

        if (response.IsSuccessStatusCode)
        {
            await localStorage.SetItemAsync("AccessToken", result.AccessToken);
            authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }

    public async Task<ClaimsPrincipal> GetUser()
    {
        var auth = await authStateProvider.GetAuthenticationStateAsync();
        return auth.User;
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("AccessToken");
        authStateProvider.MarkUserAsLoggedOut();
    }
}
