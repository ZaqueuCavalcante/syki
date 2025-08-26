namespace Syki.Back.Features.Academic.GetCoursesWithCurriculums;

public class GetCoursesWithCurriculumsService(SykiDbContext ctx) : IAcademicService
{
    public async Task<GetCoursesWithCurriculumsOut> Get(Guid institutionId)
    {
        var courses = await ctx.Courses
            .Where(c => c.InstitutionId == institutionId && c.CourseCurriculums.Count > 0)
            .OrderBy(c => c.Name)
            .ToListAsync();

        return new GetCoursesWithCurriculumsOut
        {
            Total = courses.Count,
            Items = courses.ConvertAll(c => c.ToGetCoursesWithCurriculumsItemOut())
        };
    }
}
