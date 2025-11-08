using Microsoft.AspNetCore.Components.Authorization;

namespace Exato.Front.Auth;

public class AuthManager(AuthenticationStateProvider provider)
{
    public async Task<AuthUser> GetUser()
    {
        var authState = await provider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user.Identity is not { IsAuthenticated: true })
        {
            return new AuthUser();
        }

        return new AuthUser
        {
            Id = user.Id,
            IsAuthenticated = true,
            Features = user.Features,
            Name = user.FindFirst(Claims.UserName)!.Value,
            Email = user.FindFirst(Claims.UserEmail)!.Value,
            Role = user.FindFirst(Claims.UserRole)!.Value,
        };
    }
}
