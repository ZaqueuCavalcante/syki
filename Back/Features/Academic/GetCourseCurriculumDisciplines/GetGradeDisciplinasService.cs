namespace Syki.Back.GetGradeDisciplinas;

public class GetGradeDisciplinasService(SykiDbContext ctx)
{
    public async Task<List<DisciplinaOut>> Get(Guid institutionId, Guid id)
    {
        var grade = await ctx.Grades.AsNoTracking()
            .Where(g => g.InstitutionId == institutionId && g.Id == id)
            .Include(g => g.Disciplinas)
            .FirstOrDefaultAsync();

        return grade == null ? [] : grade.Disciplinas.ConvertAll(d => d.ToOut()).OrderBy(d => d.Name).ToList();
    }
}
