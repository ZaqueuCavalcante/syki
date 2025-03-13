using Syki.Back.Features.Teacher.AddExamGradeNote;

namespace Syki.Back.Features.Cross.SeedInstitutionData;

[CommandDescription("Realizar seed de notas da instituição")]
public record SeedInstitutionExamGradeNotesCommand(Guid InstitutionId) : ICommand;

public class SeedInstitutionExamGradeNotesCommandHandler(
    SykiDbContext ctx,
    AddExamGradeNoteService addExamGradeNoteService) : ICommandHandler<SeedInstitutionExamGradeNotesCommand>
{
    public async Task Handle(Guid commandId, SeedInstitutionExamGradeNotesCommand command)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Students)
            .Include(c => c.ExamGrades)
            .Where(c => c.InstitutionId == command.InstitutionId)
            .ToListAsync();

        var random = new Random();
        foreach (var @class in classes)
        {
            foreach (var student in @class.Students)
            {
                foreach (var examGrade in @class.ExamGrades.Where(g => g.ClassId == @class.Id && g.StudentId == student.Id && g.ExamType == ExamType.N1))
                {
                    var note = Convert.ToDecimal(Math.Round(random.NextDouble()*10, 2));
                    await addExamGradeNoteService.AddWithThrowOnError(@class.TeacherId, examGrade.Id, new(note < 4 ? note+5 : note));
                }
            }
        }
    }
}
