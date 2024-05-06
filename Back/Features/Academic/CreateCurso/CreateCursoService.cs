namespace Syki.Back.Features.Academico.CreateCurso;

public class CreateCursoService(SykiDbContext ctx)
{
    public async Task<CourseOut> Create(Guid institutionId, CourseIn data)
    {
        var curso = new Curso(institutionId, data.Name, data.Type);

        ctx.Add(curso);
        await ctx.SaveChangesAsync();

        return curso.ToOut();
    }
}
