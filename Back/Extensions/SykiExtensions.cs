using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Syki.Extensions;

public static class SykiExtensions
{
    public static uint Id(this ClaimsPrincipal user)
    {
        return uint.Parse(user.FindFirstValue("sub")!);
    }

    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) { return input; }

        var startUnderscores = Regex.Match(input, "^_+");
        return startUnderscores + Regex.Replace(input, "([a-z0-9])([A-Z])", "$1_$2").ToLower();
    }

    public static void ChangeIdentityTablesToSnakeCase(this ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(entity.GetTableName()!.ToSnakeCase());

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDefaultDatabaseName()!.ToSnakeCase());
            }
        }
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
