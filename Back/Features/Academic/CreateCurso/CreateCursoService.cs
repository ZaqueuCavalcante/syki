namespace Syki.Back.Features.Academico.CreateCurso;

public class CreateCursoService(SykiDbContext ctx)
{
    public async Task<CursoOut> Create(Guid institutionId, CursoIn data)
    {
        var curso = new Curso(institutionId, data.Nome, data.Tipo);

        ctx.Add(curso);
        await ctx.SaveChangesAsync();

        return curso.ToOut();
    }
}
