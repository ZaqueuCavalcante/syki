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

    public static List<AgendaDayOut> ToAgendas(this List<MatriculaTurmaOut> turmas)
    {
        var agendas = new List<AgendaDayOut>();

        foreach (var turma in turmas)
        {
            foreach (var horario in turma.Horarios)
            {
                var disciplina = new AgendaDisciplineOut { Name = turma.Disciplina, Start = horario.Start, End = horario.End };

                var agenda = agendas.FirstOrDefault(a => a.Day == horario.Day);
                if (agenda == null)
                {
                    agenda = new AgendaDayOut { Day = horario.Day };
                    agenda.Disciplines.Add(disciplina);
                    agendas.Add(agenda);
                    continue;
                }

                agenda.Disciplines.Add(disciplina);
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
