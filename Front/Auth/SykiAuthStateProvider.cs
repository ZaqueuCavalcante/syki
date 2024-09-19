using Microsoft.JSInterop;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Auth;

public class SykiAuthStateProvider(ILocalStorageService storage) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var jwt = await storage.GetItemAsync("AccessToken");

        if (jwt.IsEmpty())
        {
            return new(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        return new(CreateClaimsPrincipalFromToken(jwt!));
    }

    public void MarkUserAsAuthenticated()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static ClaimsPrincipal CreateClaimsPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var identity = new ClaimsIdentity();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            identity = new(jwtSecurityToken.Claims, "Bearer");

            identity.AddClaim(new Claim(ClaimTypes.Role, identity.FindFirst("role")!.Value));
        }

        return new(identity);
    }
}
