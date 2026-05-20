namespace Syki.Back.Features.Disciplines.GetDisciplines;

public class GetDisciplinesService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetDisciplinesOut> Get()
    {
        var disciplines = await ctx.Disciplines.AsNoTracking()
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId)
            .OrderBy(d => d.Name)
            .ToListAsync();

        var ids = disciplines.Select(d => d.Id).ToHashSet();

        var teachers = await ctx.TeachersDisciplines.AsNoTracking()
            .Where(d => ids.Count == 0 || ids.Contains(d.DisciplineId))
            .ToListAsync();

        var result = disciplines.ConvertAll(d => d.ToGetDisciplinesItemOut());
        result.ForEach(x => x.Teachers = teachers.Count(t => t.DisciplineId == x.Id));

        return new GetDisciplinesOut { Total = result.Count, Items = result };
    }
}
