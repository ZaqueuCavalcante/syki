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

    public static Guid InstitutionId(this ClaimsPrincipal user)
    {
        // TODO: change faculdade to institutionId in the JWT
        return Guid.Parse(user.FindFirstValue("faculdade")!);
    }

    public static bool IsAuditable(this PathString path)
    {
        return path != "/login" && path != "/users/login-mfa" && path != "/users/reset-password" && path != "/demos"  && path != "/demos/setup";
    }
}
