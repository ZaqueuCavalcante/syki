namespace Syki.Back.Features.Academic.GetCoursesWithCurriculums;

public class GetCoursesWithCurriculumsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<List<CreateCourseOut>> Get(Guid institutionId)
    {
        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == institutionId && c.CourseCurriculums.Count > 0)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return courses.ConvertAll(c => c.ToOut());
    }
}
