using Syki.Shared;
using Syki.Front.Auth;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Syki.Front.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _http;
    private readonly ILocalStorageService _localStorage;
    private readonly SykiAuthStateProvider _authStateProvider;

    public AuthService(
        HttpClient http,
        ILocalStorageService localStorage,
        SykiAuthStateProvider authStateProvider
    ) {
        _http = http;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<LoginOut> Login(string email, string password)
    {
        var body = new LoginIn { Email = email, Password = password };
        var response = await _http.PostAsJsonAsync("/users/login", body);

        var responseAsString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginOut>(responseAsString)!;

        if (response.IsSuccessStatusCode)
        {
            await _localStorage.SetItemAsync("AccessToken", result.AccessToken);
            _authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }

    public async Task<LoginOut> LoginMfa(string code)
    {
        var body = new LoginMfaIn { Code = code };
        var response = await _http.PostAsJsonAsync("/users/login-mfa", body);

        var responseAsString = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<LoginOut>(responseAsString)!;

        if (response.IsSuccessStatusCode)
        {
            await _localStorage.SetItemAsync("AccessToken", result.AccessToken);
            _authStateProvider.MarkUserAsAuthenticated();
        }

        return result;
    }

    public async Task<ClaimsPrincipal> GetUser()
    {
        var auth = await _authStateProvider.GetAuthenticationStateAsync();
        return auth.User;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("AccessToken");
        _authStateProvider.MarkUserAsLoggedOut();
    }
}
