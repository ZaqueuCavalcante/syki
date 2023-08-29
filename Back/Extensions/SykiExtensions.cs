using System.Security.Claims;

namespace Syki.Extensions;

public static class SykiExtensions
{
    public static uint Id(this ClaimsPrincipal user)
    {
        return uint.Parse(user.FindFirstValue("sub")!);
    }

    public static long Facul(this ClaimsPrincipal user)
    {
        return long.Parse(user.FindFirstValue("faculdade")!);
    }

    public static bool IsEmpty(this string? text)
    {
        return string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);
    }

    public static bool HasValue(this string? text)
    {
        return !string.IsNullOrEmpty(text);
    }
}
