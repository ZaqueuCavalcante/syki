namespace Syki.Back.Features.Academic.GetCoursesWithDisciplines;

public class GetCoursesWithDisciplinesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<GetCoursesWithDisciplinesOut> Get(Guid institutionId)
    {
        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == institutionId && c.Disciplines.Count > 0)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new GetCoursesWithDisciplinesOut
        {
            Total = courses.Count,
            Items = courses.ConvertAll(c => c.ToGetCoursesWithDisciplinesItemOut())
        };
    }
}
