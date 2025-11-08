namespace Syki.Shared;

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

    public static List<AgendaDayOut> ToAgendas(this List<EnrollmentClassOut> classes)
    {
        var agendas = new List<AgendaDayOut>();

        foreach (var @class in classes)
        {
            foreach (var schedule in @class.Schedules)
            {
                var discipline = new AgendaDisciplineOut { ClassId = @class.Id, Name = @class.Discipline, Start = schedule.StartAt, End = schedule.EndAt };

                var agenda = agendas.FirstOrDefault(a => a.Day == schedule.Day);
                if (agenda == null)
                {
                    agenda = new AgendaDayOut { Day = schedule.Day };
                    agenda.Disciplines.Add(discipline);
                    agendas.Add(agenda);
                    continue;
                }

                agenda.Disciplines.Add(discipline);
            }
        }

        agendas = agendas.OrderBy(a => a.Day).ToList();
        foreach (var agenda in agendas)
        {
            agenda.Disciplines = agenda.Disciplines.OrderBy(d => d.Start).ToList();
        }

        return agendas;
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
