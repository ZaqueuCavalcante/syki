namespace Syki.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(SykiDbContext ctx)
{
    public async Task<StudentInsightsOut> Get(Guid courseCurriculumId)
    {
        var totalDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(g => g.CourseCurriculumId == courseCurriculumId)
            .CountAsync();

        return new() { TotalDisciplines = totalDisciplines };
    }
}
