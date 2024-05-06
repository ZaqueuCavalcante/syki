namespace Syki.Back.GetGrades;

public class GetGradesService(SykiDbContext ctx)
{
    public async Task<List<GradeOut>> Get(Guid institutionId)
    {
        var grades = await ctx.Grades.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .Include(g => g.Curso)
            .Include(g => g.Disciplinas)
            .Include(g => g.Vinculos)
            .OrderBy(g => g.Name)
            .ToListAsync();

        return grades.ConvertAll(g => g.ToOut());
    }
}
