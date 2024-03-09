namespace Syki.Back.GetCursos;

public class GetCursosService(SykiDbContext ctx)
{
    public async Task<List<CursoOut>> Get(Guid institutionId)
    {
        var cursos = await ctx.Cursos
            .Where(c => c.FaculdadeId == institutionId)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }
}
