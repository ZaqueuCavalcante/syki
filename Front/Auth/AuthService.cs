using Syki.Dtos;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using System.Security.Claims;

namespace Syki.Front.Auth;

public class AuthService
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

    public async Task<bool> Login(string email, string password)
    {
        var body = new LoginIn { Email = email, Password = password };
        var response = await _http.PostAsJsonAsync("/users/login", body);

        if (response.IsSuccessStatusCode)
        {
            var responseAsString = await response.Content.ReadAsStringAsync();
            var jwt = JsonConvert.DeserializeObject<LoginOut>(responseAsString)!.AccessToken;

            await _localStorage.SetItemAsync("AccessToken", jwt);

            _authStateProvider.MarkUserAsAuthenticated();

            return true;
        }

        return false;
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
