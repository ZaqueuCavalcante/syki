using Syki.Back.Features.Teacher.AddClassActivityNote;

namespace Syki.Back.Features.Cross.SeedInstitutionData;

[CommandDescription("Realizar seed de notas da instituição")]
public record SeedInstitutionClassActivitiesNotesCommand(Guid InstitutionId) : ICommand;

public class SeedInstitutionClassActivitiesNotesCommandHandler(
    SykiDbContext ctx,
    AddClassActivityNoteService AddClassActivityNoteService) : ICommandHandler<SeedInstitutionClassActivitiesNotesCommand>
{
    public async Task Handle(Guid commandId, SeedInstitutionClassActivitiesNotesCommand command)
    {
        var classes = await ctx.Classes.AsNoTracking()
            .Include(c => c.Students)
            .Include(c => c.Notes)
            .Where(c => c.InstitutionId == command.InstitutionId)
            .ToListAsync();

        var random = new Random();
        foreach (var @class in classes)
        {
            foreach (var student in @class.Students)
            {
                foreach (var note in @class.Notes.Where(g => g.ClassId == @class.Id && g.StudentId == student.Id && g.Type == StudentClassNoteType.N1))
                {
                    var value = Convert.ToDecimal(Math.Round(random.NextDouble()*10, 2));
                    await AddClassActivityNoteService.AddWithThrowOnError(@class.TeacherId, note.Id, new(value < 4 ? value+5 : value));
                }
            }
        }
    }
}
