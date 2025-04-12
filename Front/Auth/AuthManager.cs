using Microsoft.AspNetCore.Components.Authorization;

namespace Syki.Front.Auth;

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
            IsAuthenticated = true,
            Id = Guid.Parse(user.FindFirst("sub").Value),
            Name = user.FindFirst("name").Value,
            Email = user.FindFirst("email").Value,
            Role = Enum.Parse<UserRole>(user.FindFirst("role").Value),
        };
    }
}
