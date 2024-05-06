namespace Syki.Back.CreateMatriculaAluno;

public class CreateMatriculaAlunoService(SykiDbContext ctx)
{
    public async Task Create(Guid institutionId, Guid userId, MatriculaTurmaIn data)
    {
        var ids = await ctx.Turmas
            .Where(t => t.InstitutionId == institutionId && data.Turmas.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

        var turmas = await ctx.TurmaAlunos.Where(x => x.AlunoId == userId).ToListAsync();
        ctx.RemoveRange(turmas);

        foreach (var id in ids)
        {
            ctx.Add(new TurmaAluno(id, userId, Situacao.Matriculado));
        }

        await ctx.SaveChangesAsync();
    }
}
