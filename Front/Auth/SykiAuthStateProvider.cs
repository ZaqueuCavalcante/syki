using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Auth;

public class SykiAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService _localStorage;

    public SykiAuthStateProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwt = await _localStorage.GetItemAsync("AccessToken");

        if (string.IsNullOrWhiteSpace(jwt))
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        
        return new AuthenticationState(CreateClaimsPrincipalFromToken(jwt));
    }

    public void MarkUserAsAuthenticated()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            identity = new(jwtSecurityToken.Claims, "Bearer");
        }

        return new(identity);
    }
}

public static class ApiExtensions
{
    public static async Task<T> DeserializeTo<T>(this HttpResponseMessage httpResponse)
    {
        var responseAsString = await httpResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseAsString)!;
    }
}
