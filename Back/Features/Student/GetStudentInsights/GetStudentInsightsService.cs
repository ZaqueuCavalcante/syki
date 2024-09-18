namespace Syki.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(SykiDbContext ctx) : IStudentService
{
    public async Task<StudentInsightsOut> Get(Guid userId, Guid courseCurriculumId)
    {
        var totalDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(g => g.CourseCurriculumId == courseCurriculumId)
            .CountAsync();

        var finishedDisciplines = await ctx.ClassesStudents
            .Where(x => x.SykiStudentId == userId && x.StudentDisciplineStatus == StudentDisciplineStatus.Aprovado)
            .CountAsync();

        var attendances = await ctx.Attendances.AsNoTracking().Where(x => x.StudentId == userId).ToListAsync();
        var totalPresences = attendances.Count(x => x.Present);
        var frequency = (attendances.Count == 0) ? 0.00M : 100M*(1M * totalPresences / (1M * attendances.Count));

        return new() { Status = StudentStatus.Enrolled, TotalDisciplines = totalDisciplines, FinishedDisciplines = finishedDisciplines, Frequency = frequency };
    }
}
