using System.Security.Claims;

namespace Syki.Back.Extensions;

public static class UserExtensions
{
    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("sub")!);
    }

    public static Guid Facul(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("faculdade")!);
    }

    public static bool IsLogin(this PathString path)
    {
        return path == "/users/login" || path == "/users/login-mfa";
    }
}
