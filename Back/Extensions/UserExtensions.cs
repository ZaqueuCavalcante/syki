using System.Security.Claims;

namespace Syki.Back.Extensions;

public static class UserExtensions
{
    public static Guid InstitutionId(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("institution")!);
    }

    public static Guid Id(this ClaimsPrincipal user)
    {
        return Guid.Parse(user.FindFirstValue("sub")!);
    }

    public static bool IsAuditable(this PathString path)
    {
        return
            path != "/login" &&
            path != "/login/mfa" &&
            path != "/reset-password" &&
            path != "/users" &&
            path != "/reset-password-token";
    }
}
