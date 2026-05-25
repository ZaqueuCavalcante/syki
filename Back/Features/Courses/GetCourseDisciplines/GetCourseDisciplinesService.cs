namespace Syki.Back.Features.Courses.GetCourseDisciplines;

public class GetCourseDisciplinesService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetCourseDisciplinesOut> Get(int id)
    {
        var ids = await ctx.CoursesDisciplines
            .Where(x => x.CourseId == id).Select(x => x.DisciplineId).ToListAsync();

        var disciplines = await ctx.Disciplines
            .Where(d => d.InstitutionId == ctx.RequestUser.InstitutionId && ids.Contains(d.Id))
            .Select(d => new GetCourseDisciplineItemOut { Id = d.Id, Name = d.Name })
            .OrderBy(d => d.Name)
            .ToListAsync();

        return new GetCourseDisciplinesOut { Total = disciplines.Count, Items = disciplines };
    }
}
