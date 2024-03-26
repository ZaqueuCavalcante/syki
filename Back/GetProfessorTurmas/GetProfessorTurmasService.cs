namespace Syki.Back.GetProfessorTurmas;

public class GetProfessorTurmasService(SykiDbContext ctx)
{
    public async Task<List<ProfessorTurmaOut>> Get(Guid institutionId, Guid userId)
    {
        var turmas = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(t => t.FaculdadeId == institutionId && t.ProfessorId == userId)
            .ToListAsync();

        return turmas.ConvertAll(t => t.ToProfessorTurmaOut());
    }
}
