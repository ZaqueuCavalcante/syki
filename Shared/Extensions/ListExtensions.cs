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

    public static List<AgendaDiaOut> ToAgendas(this List<MatriculaTurmaOut> turmas)
    {
        var agendas = new List<AgendaDiaOut>();

        foreach (var turma in turmas)
        {
            foreach (var horario in turma.Horarios)
            {
                var disciplina = new AgendaDisciplinaOut { Nome = turma.Disciplina, Start = horario.Start, End = horario.End };

                var agenda = agendas.FirstOrDefault(a => a.Dia == horario.Dia);
                if (agenda == null)
                {
                    agenda = new AgendaDiaOut { Dia = horario.Dia };
                    agenda.Disciplinas.Add(disciplina);
                    agendas.Add(agenda);
                    continue;
                }

                agenda.Disciplinas.Add(disciplina);
            }
        }

        agendas = agendas.OrderBy(a => a.Dia).ToList();
        foreach (var agenda in agendas)
        {
            agenda.Disciplinas = agenda.Disciplinas.OrderBy(d => d.Start).ToList();
        }

        return agendas;
    }
}
