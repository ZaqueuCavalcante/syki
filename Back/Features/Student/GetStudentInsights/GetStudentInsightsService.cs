namespace Syki.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(SykiDbContext ctx) : IStudentService
{
    public async Task<StudentInsightsOut> Get(Guid userId, Guid courseCurriculumId)
    {
        var totalDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(g => g.CourseCurriculumId == courseCurriculumId)
            .CountAsync();

        // TODO: Adicionar demais props...

        var notas = await ctx.ExamGrades.Where(g => g.StudentId == userId).ToListAsync();
        var ids = notas.Select(g => g.ClassId).Distinct().ToList();
        var classes = await ctx.Classes
            .Select(x => new { x.Id, x.Workload })
            .Where(g => ids.Contains(g.Id))
            .ToListAsync();


        return new() { TotalDisciplines = totalDisciplines };
    }
}
