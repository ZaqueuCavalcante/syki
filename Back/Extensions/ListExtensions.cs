namespace Syki.Back.Extensions;

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
}
