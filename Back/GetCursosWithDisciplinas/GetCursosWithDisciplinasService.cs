namespace Syki.Back.GetCursosWithDisciplinas;

public class GetCursosWithDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<CursoOut>> Get(Guid institutionId)
    {
        var cursos = await ctx.Cursos
            .Where(c => c.FaculdadeId == institutionId && c.Disciplinas.Count > 0)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }
}
