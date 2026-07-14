namespace Estud.Back.Features.Courses.GetCourses;

public class GetCoursesService(EstudDbContext ctx) : IEstudService
{
    private const int MaxPageSize = 100;

    public async Task<GetCoursesOut> Get(GetCoursesIn query)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, MaxPageSize);

        var institutionId = ctx.RequestUser.InstitutionId;

        var coursesQuery = ctx.Courses.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId);

        var total = await coursesQuery.CountAsync();

        var courses = await coursesQuery
            .OrderBy(c => c.Name)
            .ThenBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var ids = courses.Select(c => c.Id).ToHashSet();

        var result = courses.ConvertAll(c => c.ToGetCoursesItemOut());

        if (ids.Count > 0)
        {
            var disciplines = await ctx.CoursesDisciplines.AsNoTracking()
                .Where(x => ids.Contains(x.CourseId))
                .ToListAsync();
            result.ForEach(x => x.Disciplines = disciplines.Count(d => d.CourseId == x.Id));
        }

        return new GetCoursesOut
        {
            Total = total,
            Page = page,
            PageSize = pageSize,
            Items = result,
        };
    }
}
