namespace Syki.Back.Features.Student.CreateClassActivityWork;

[DomainEvent(nameof(ClassActivityWork), "Nota adicionada")]
public record StudentClassNoteAddedDomainEvent(Guid StudentId, Guid ClassActivityId) : IDomainEvent;

public class StudentClassNoteAddedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentClassNoteAddedDomainEvent>
{
    public async Task Handle(Guid institutionId, DomainEventId eventId, StudentClassNoteAddedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new CreateNewStudentClassNoteNotificationCommand(evt.ClassActivityId, evt.StudentId), eventId: eventId);
        await Task.CompletedTask;
    }
}
