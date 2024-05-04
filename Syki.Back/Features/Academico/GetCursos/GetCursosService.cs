namespace Syki.Back.Features.Academico.GetCursos;

public class GetCursosService(SykiDbContext ctx)
{
    public async Task<List<CursoOut>> Get(Guid institutionId)
    {
        var cursos = await ctx.Cursos
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }
}
