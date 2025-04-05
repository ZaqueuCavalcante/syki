using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Auth;

public class AuthManager(AuthenticationStateProvider provider)
{
    public async Task<AuthUser> GetUser()
    {
        var authState = await provider.GetAuthenticationStateAsync();

        if (authState.User.Identity is not { IsAuthenticated: true })
        {
            return new AuthUser();
        }

        return new AuthUser
        {
            Name = authState.User.FindFirst("name").Value,
            Email = authState.User.FindFirst("email").Value,
            Role = Enum.Parse<UserRole>(authState.User.FindFirst("role").Value),
        };
    }
}
