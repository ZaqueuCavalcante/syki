using Syki.Back.Features.Student.GetStudentFrequency;
using Syki.Back.Features.Student.GetStudentAverageNote;

namespace Syki.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(SykiDbContext ctx, GetStudentFrequencyService frequencyService, GetStudentAverageNoteService averageNoteService) : IStudentService
{
    public async Task<StudentInsightsOut> Get(Guid userId, Guid courseCurriculumId)
    {
        var student = await ctx.Students.FirstAsync(x => x.Id == userId);

        var totalDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(g => g.CourseCurriculumId == courseCurriculumId)
            .CountAsync();
        var finishedDisciplines = await ctx.ClassesStudents
            .Where(x => x.SykiStudentId == userId && x.StudentDisciplineStatus == StudentDisciplineStatus.Aprovado)
            .CountAsync();
        
        var frequency = await frequencyService.Get(userId);

        var averageNote = await averageNoteService.Get(userId);

        return new()
        {
            Status = student.Status,
            TotalDisciplines = totalDisciplines,
            FinishedDisciplines = finishedDisciplines,
            Frequency = frequency.Frequency,
            Average = averageNote,
        };
    }
}
