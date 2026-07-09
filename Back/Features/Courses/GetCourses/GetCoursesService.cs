namespace Estud.Back.Features.Courses.GetCourses;

public class GetCoursesService(EstudDbContext ctx) : IEstudService
{
    public async Task<GetCoursesOut> Get()
    {
        var institutionId = ctx.RequestUser.InstitutionId;

        var courses = await ctx.Courses.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        var ids = courses.Select(c => c.Id).ToHashSet();

        var result = courses.ConvertAll(c => c.ToGetCoursesItemOut());

        var disciplines = await ctx.CoursesDisciplines.AsNoTracking()
            .Where(x => ids.Count == 0 || ids.Contains(x.CourseId))
            .ToListAsync();
        result.ForEach(x => x.Disciplines = disciplines.Count(d => d.CourseId == x.Id));

        return new GetCoursesOut { Total = result.Count, Items = result };
    }
}
