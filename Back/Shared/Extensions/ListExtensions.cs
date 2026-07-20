namespace Estud.Back.Shared;

public static class ListExtensions
{
    public static bool IsSubsetOf(this List<Guid> selfs, List<Guid> others)
    {
        HashSet<Guid> set = [];
        foreach (var self in selfs)
        {
            if (!set.Add(self)) return false;

            if (!others.Contains(self)) return false;
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

    public static bool IsEquivalentTo(this List<Guid> selfs, List<Guid> others)
    {
        if (selfs.Count != others.Count) return false;

        return selfs.IsSubsetOf(others);
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
        return source.OrderBy(x => Guid.CreateVersion7());
    }
}
