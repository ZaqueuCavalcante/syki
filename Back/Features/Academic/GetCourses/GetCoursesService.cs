namespace Syki.Back.Features.Academic.GetCourses;

public class GetCoursesService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<CourseOut>> Get(Guid institutionId)
    {
        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return courses.ConvertAll(c => c.ToOut());
    }
}
