using Microsoft.JSInterop;
using System.Security.Claims;
using Exato.Shared.Features.Cross.GetUserAccount;
using Microsoft.AspNetCore.Components.Authorization;

namespace Exato.Front.Auth;

public class ExatoAuthStateProvider(ILocalStorageService storage) : AuthenticationStateProvider
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

        identity.AddClaim(new Claim(Claims.UserId, user.Id.ToString()));
        identity.AddClaim(new Claim(Claims.UserName, user.Name));
        identity.AddClaim(new Claim(Claims.UserEmail, user.Email));
        identity.AddClaim(new Claim(Claims.UserRole, user.Role.ToString()));
        identity.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()));
        identity.AddClaim(new Claim(Claims.UserFeatures, user.Features.Serialize()));

        return new(identity);
    }
}
