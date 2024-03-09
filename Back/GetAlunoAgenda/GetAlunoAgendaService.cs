namespace Syki.Back.GetAlunoAgenda;

public class GetAlunoAgendaService(SykiDbContext ctx)
{
    public async Task<List<AgendaDiaOut>> Get(Guid institution, Guid userId)
    {
        var ids = await ctx.TurmaAlunos.AsNoTracking()
            .Where(x => x.AlunoId == userId && x.Situacao == Situacao.Matriculado)
            .Select(x => x.TurmaId)
            .ToListAsync();

        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(t => t.FaculdadeId == institution && ids.Contains(t.Id))
            .ToListAsync();

        var response = turmas.ConvertAll(t =>
        {
            return new MatriculaTurmaOut
            {
                Disciplina = t.Disciplina.Nome,
                Horarios = t.Horarios.ConvertAll(h => h.ToOut()),
            };
        });
        
        return response.ToAgendas();
    }
}
