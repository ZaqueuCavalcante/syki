using Exato.Shared.Auth;

namespace Exato.Shared.Extensions;

public static class ListExtensions
{
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }

    public static string ToStringList<TEnum>(this IEnumerable<TEnum> list) where TEnum : Enum
    {
        return string.Join(", ", list.Select(e => e.ToString()));
    }

    public static string ToStringList(this IEnumerable<ExatoFeature> list, string? separator = " | ")
    {
        return string.Join(separator, list.Select(e => e.Name));
    }

    public static bool IsAllDistinct(this IEnumerable<int> list)
    {
        if (list is null) return true;

        var set = new HashSet<int>();
        foreach (var x in list)
        {
            if (!set.Add(x)) return false;
        }

        return true;
    }

    public static bool IsAllDistinct(this IEnumerable<string> list)
    {
        if (list is null) return true;

        var set = new HashSet<string>();
        foreach (var x in list)
        {
            if (!set.Add(x)) return false;
        }

        return true;
    }

    public static bool IsSubsetOf(this List<int> selfs, List<int> others)
    {
        HashSet<int> set = [];
        foreach (var self in selfs)
        {
            if (!set.Add(self)) return false;

            if (!others.Contains(self)) return false;
        }

        return true;
    }

    public static string ToSqlInList(this IEnumerable<string> values)
    {
        if (values == null || !values.Any())
            return "()";

        var escaped = values.Select(v => v.Replace("'", "''"));

        return $"('{string.Join("','", escaped)}')";
    }
}
