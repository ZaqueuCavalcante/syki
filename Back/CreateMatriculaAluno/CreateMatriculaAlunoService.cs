namespace Syki.Back.CreateMatriculaAluno;

public class CreateMatriculaAlunoService(SykiDbContext ctx)
{
    public async Task Create(Guid institutionId, Guid userId, MatriculaTurmaIn data)
    {
        var ids = await ctx.Turmas
            .Where(t => t.FaculdadeId == institutionId && data.Turmas.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        foreach (var id in ids)
        {
            ctx.Add(new TurmaAluno(id, userId, Situacao.Matriculado));
        }

        await ctx.SaveChangesAsync();
    }
}
