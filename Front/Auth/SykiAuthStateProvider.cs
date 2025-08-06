using Microsoft.JSInterop;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Auth;

public class SykiAuthStateProvider(ILocalStorageService storage) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await storage.GetItemAsync<GetUserAccountOut>("User");

        if (user == null)
        {
            return new(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        return new(CreateClaimsPrincipalFromToken(user));
    }

    public void MarkUserAsAuthenticated()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public void MarkUserAsLoggedOut()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    private static ClaimsPrincipal CreateClaimsPrincipalFromToken(GetUserAccountOut user)
    {
        var identity = new ClaimsIdentity("Bearer");

        identity.AddClaim(new Claim("sub", user.Id.ToString()));
        identity.AddClaim(new Claim("name", user.Name));
        identity.AddClaim(new Claim("email", user.Email));
        identity.AddClaim(new Claim("role", user.Role.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));

        return new(identity);
    }
}
