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

    public static string Email(this ClaimsPrincipal user)
    {
        return user.FindFirstValue("email")!;
    }

    public static Guid InstitutionId(this ClaimsPrincipal user)
    {
        // TODO: change faculdade to institutionId in the JWT
        return Guid.Parse(user.FindFirstValue("faculdade")!);
    }

    public static bool IsAuditable(this PathString path)
    {
        return path != "/login" && path != "/login-with-google" && path != "/mfa/login" && path != "/reset-password" && path != "/user-register"  && path != "/demos/setup";
    }
}
