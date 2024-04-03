namespace Syki.Back.GetProfessorTurma;

public class GetProfessorTurmaService(SykiDbContext ctx)
{
    public async Task<ProfessorTurmaOut> Get(Guid institutionId, Guid userId, string turmaId)
    {
        _ = Guid.TryParse(turmaId, out var id);
        var turma = await ctx.Turmas.AsNoTracking()
            .Include(t => t.Disciplina)
            .Include(t => t.Horarios)
            .Where(t => t.FaculdadeId == institutionId && t.ProfessorId == userId && t.Id == id)
            .FirstOrDefaultAsync();

        if (turma == null)
            Throw.DE029.Now();

        return turma.ToProfessorTurmaOut();
    }
}
