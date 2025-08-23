namespace Syki.Back.Features.Academic.GetCoursesWithDisciplines;

public class GetCoursesWithDisciplinesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<CreateCourseOut>> Get(Guid institutionId)
    {
        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == institutionId && c.Disciplines.Count > 0)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return courses.ConvertAll(c => c.ToOut());
    }
}
