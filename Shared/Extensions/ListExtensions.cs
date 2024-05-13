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

    public static List<AgendaDayOut> ToAgendas(this List<EnrollmentClassOut> classes)
    {
        var agendas = new List<AgendaDayOut>();

        foreach (var @class in classes)
        {
            foreach (var schedule in @class.Schedules)
            {
                var discipline = new AgendaDisciplineOut { Name = @class.Discipline, Start = schedule.Start, End = schedule.End };

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
}
