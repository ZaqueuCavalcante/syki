namespace Syki.Back.Features.Academic.GetCourseDisciplines;

public class GetCourseDisciplinesService(SykiDbContext ctx)
{
    public async Task<List<CourseDisciplineOut>> Get(Guid id, Guid institutionId)
    {
        var ids = await ctx.CoursesDisciplines
            .Where(x => x.CourseId == id).Select(x => x.DisciplineId).ToListAsync();

        var disciplines = await ctx.Disciplines
            .Where(d => d.InstitutionId == institutionId && ids.Contains(d.Id))
            .Select(d => new CourseDisciplineOut { Id = d.Id, Name = d.Name })
            .OrderBy(d => d.Name)
            .ToListAsync();

        return disciplines;
    }
}
