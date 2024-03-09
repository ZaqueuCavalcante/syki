namespace Syki.Back.GetTurmas;

public class GetTurmasService(SykiDbContext ctx)
{
    public async Task<List<TurmaOut>> Get(Guid institutionId)
    {
        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Professor)
            .Include(t => t.Horarios)
            .Where(c => c.FaculdadeId == institutionId)
            .ToListAsync();

        return turmas.ConvertAll(t => t.ToOut());
    }
}
