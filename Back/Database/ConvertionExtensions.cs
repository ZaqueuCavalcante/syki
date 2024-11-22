using System.Text;
using System.Globalization;

namespace Syki.Back.Database;

public static class ConvertionExtensions
{
    private static CultureInfo _culture;

    public static void ToSnakeCaseNames(this ModelBuilder modelBuilder)
    {
        _culture = CultureInfo.InvariantCulture;

        SetNames(modelBuilder);
    }

    private static string? NameRewriter(this string name)
    {
        if (string.IsNullOrEmpty(name)) return name;

        return SnakeCaseNameRewriter(name);
    }

    private static void SetNames(ModelBuilder modelBuilder)
    {
        _culture = CultureInfo.InvariantCulture;

        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            entity.SetViewName(entity.GetViewName()?.NameRewriter());
            entity.SetSchema(entity.GetSchema()?.NameRewriter());
            entity.SetTableName(entity.GetTableName()?.NameRewriter());

            foreach (var property in entity!.GetProperties())
            {
                property.SetColumnName(property.GetColumnName()?.NameRewriter());
            }

            foreach (var key in entity.GetKeys())
            {
                key.SetName(key.GetName()?.NameRewriter());
            }

            foreach (var key in entity.GetForeignKeys())
            {
                key.SetConstraintName(key.GetConstraintName()?.NameRewriter());
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.NameRewriter());
            }
        }
    }

    // https://github.com/efcore/EFCore.NamingConventions/blob/main/EFCore.NamingConventions/Internal/SnakeCaseNameRewriter.cs
    private static string SnakeCaseNameRewriter(string name)
    {
        var builder = new StringBuilder(name.Length + Math.Min(2, name.Length / 5));
        var previousCategory = default(UnicodeCategory?);

        for (var currentIndex = 0; currentIndex < name.Length; currentIndex++)
        {
            var currentChar = name[currentIndex];
            if (currentChar == '_')
            {
                builder.Append('_');
                previousCategory = null;
                continue;
            }

            var currentCategory = char.GetUnicodeCategory(currentChar);
            switch (currentCategory)
            {
                case UnicodeCategory.UppercaseLetter:
                case UnicodeCategory.TitlecaseLetter:
                    if (previousCategory == UnicodeCategory.SpaceSeparator ||
                        previousCategory == UnicodeCategory.LowercaseLetter ||
                        previousCategory != UnicodeCategory.DecimalDigitNumber &&
                        previousCategory != null &&
                        currentIndex > 0 &&
                        currentIndex + 1 < name.Length &&
                        char.IsLower(name[currentIndex + 1]))
                    {
                        builder.Append('_');
                    }

                    currentChar = char.ToLower(currentChar, _culture);
                    break;

                case UnicodeCategory.LowercaseLetter:
                case UnicodeCategory.DecimalDigitNumber:
                    if (previousCategory == UnicodeCategory.SpaceSeparator)
                    {
                        builder.Append('_');
                    }
                    break;

                default:
                    if (previousCategory != null)
                    {
                        previousCategory = UnicodeCategory.SpaceSeparator;
                    }
                    continue;
            }

            builder.Append(currentChar);
            previousCategory = currentCategory;
        }

        return builder.ToString().ToLower(_culture);
    }
}
