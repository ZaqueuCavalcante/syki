namespace Syki.Back.GetGrades;

public class GetGradesService(SykiDbContext ctx)
{
    public async Task<List<GradeOut>> Get(Guid institutionId)
    {
        var grades = await ctx.Grades.AsNoTracking()
            .Where(c => c.FaculdadeId == institutionId)
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .OrderBy(g => g.Nome)
            .ToListAsync();

        return grades.ConvertAll(g => g.ToOut());
    }
}
