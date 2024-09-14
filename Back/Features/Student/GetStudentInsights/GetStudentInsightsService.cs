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

        // Filtrar so as relacionadas com o studante
        var lessons = await ctx.Lessons.Include(x => x.Attendances).ToListAsync();
        var lessonsCount = lessons.Count(x => x.Attendances.Count > 0);
        var presences = lessons.Count(x => x.Attendances.Exists(a => a.StudentId == userId && a.Present));
        var frequency = lessonsCount == 0 ? 0.00M : 100M * (1M * presences / (1M * lessonsCount));

        return new() { Status = StudentStatus.Enrolled, TotalDisciplines = totalDisciplines, FinishedDisciplines = finishedDisciplines, Frequency = frequency };
    }
}
