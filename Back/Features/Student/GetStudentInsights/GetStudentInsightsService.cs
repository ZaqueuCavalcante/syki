namespace Syki.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(SykiDbContext ctx)
{
    public async Task<StudentInsightsOut> Get(Guid userId)
    {
        var courseOfferingId = await ctx.Students.Where(a => a.Id == userId)
            .Select(a => a.CourseOfferingId).FirstAsync();
        var courseCurriculumId = await ctx.CourseOfferings.Where(o => o.Id == courseOfferingId)
            .Select(o => o.CourseCurriculumId).FirstAsync();
        var totalDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(g => g.CourseCurriculumId == courseCurriculumId)
            .CountAsync();

        return new() { TotalDisciplines = totalDisciplines };
    }
}
