namespace Syki.Back.Features.Courses.GetCourses;

public class GetCoursesService(SykiDbContext ctx) : ISykiService
{
    public async Task<GetCoursesOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var courses = await ctx.Courses.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new GetCoursesOut
        {
            Total = courses.Count,
            Items = courses.ConvertAll(c => c.ToGetCoursesItemOut())
        };
    }
}
