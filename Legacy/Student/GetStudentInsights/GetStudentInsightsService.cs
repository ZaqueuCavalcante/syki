using Estud.Back.Features.Student.GetStudentFrequency;
using Estud.Back.Features.Student.GetStudentAverageNote;

namespace Estud.Back.Features.Student.GetStudentInsights;

public class GetStudentInsightsService(EstudDbContext ctx, GetStudentFrequencyService frequencyService, GetStudentAverageNoteService averageNoteService) : IEstudService
{
    public async Task<StudentInsightsOut> Get(Guid userId, Guid courseCurriculumId)
    {
        var student = await ctx.Students.FirstAsync(x => x.Id == userId);

        var totalDisciplines = await ctx.CourseCurriculumDisciplines.AsNoTracking()
            .Where(g => g.CourseCurriculumId == courseCurriculumId)
            .CountAsync();
        var finishedDisciplines = await ctx.ClassesStudents
            .Where(x => x.EstudStudentId == userId && x.StudentDisciplineStatus == StudentDisciplineStatus.Aprovado)
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
