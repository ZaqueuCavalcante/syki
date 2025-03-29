namespace Syki.Back.Features.Teacher.AddClassActivityNote;

[DomainEvent(nameof(StudentClassNote), "Nota adicionada")]
public record StudentClassNoteAddedDomainEvent(Guid StudentId, Guid ClassId) : IDomainEvent;

public class StudentClassNoteAddedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentClassNoteAddedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, StudentClassNoteAddedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new CreateNewStudentClassNoteNotificationCommand(evt.ClassId, evt.StudentId), eventId: eventId);
        await Task.CompletedTask;
    }
}
