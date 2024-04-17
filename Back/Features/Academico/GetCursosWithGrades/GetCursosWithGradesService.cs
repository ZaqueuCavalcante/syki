namespace Syki.Back.GetCursosWithGrades;

public class GetCursosWithGradesService(SykiDbContext ctx)
{
    public async Task<List<CursoOut>> Get(Guid institutionId)
    {
        var cursos = await ctx.Cursos
            .Where(c => c.InstitutionId == institutionId && c.Grades.Count > 0)
            .OrderBy(c => c.Nome)
            .ToListAsync();

        return cursos.ConvertAll(c => c.ToOut());
    }
}
