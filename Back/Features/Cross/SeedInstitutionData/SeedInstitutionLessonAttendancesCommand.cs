using Syki.Back.Features.Teacher.CreateLessonAttendance;

namespace Syki.Back.Features.Cross.SeedInstitutionData;

[CommandDescription("Realizar seed de chamadas da instituição")]
public record SeedInstitutionLessonAttendancesCommand(Guid InstitutionId) : ICommand;

public class SeedInstitutionLessonAttendancesCommandHandler(
    SykiDbContext ctx,
    CreateLessonAttendanceService createLessonAttendanceService) : ICommandHandler<SeedInstitutionLessonAttendancesCommand>
{
    public async Task Handle(Guid commandId, SeedInstitutionLessonAttendancesCommand command)
    {
        var id = command.InstitutionId;

        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Lessons)
            .Include(c => c.Students)
            .Where(c => c.InstitutionId == id)
            .ToListAsync();

        var random = new Random();
        var today = DateTime.Now.ToDateOnly();
        foreach (var @class in classes)
        {
            foreach (var lesson in @class.Lessons.Where(l => l.Date < today))
            {
                var presentStudents = @class.Students.Select(s => s.Id).PickRandom(random.Next(3, 7)).ToList();
                await createLessonAttendanceService.CreateWithThrowOnError(@class.TeacherId, lesson.Id, new(presentStudents));
            }
        }

        ctx.AddCommand(id, new SeedInstitutionExamGradeNotesCommand(id), parentId: commandId);
    }
}
