namespace Syki.Back.GetCursosWithDisciplinas;

public class GetCursosWithDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<CourseOut>> Get(Guid institutionId)
    {
        var cursos = await ctx.Cursos
            .Where(c => c.InstitutionId == institutionId && c.Disciplinas.Count > 0)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }
}
