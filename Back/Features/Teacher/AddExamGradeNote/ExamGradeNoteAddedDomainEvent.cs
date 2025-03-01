namespace Syki.Back.Features.Teacher.AddExamGradeNote;

[DomainEventDescription("Nota adicionada")]
public record ExamGradeNoteAddedDomainEvent(Guid StudentId, Guid ClassId) : IDomainEvent;

public class ExamGradeNoteAddedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ExamGradeNoteAddedDomainEvent>
{
    public async Task Handle(Guid eventId, Guid institutionId, ExamGradeNoteAddedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(eventId, institutionId, new CreateNewExamGradeNoteNotificationCommand(evt.ClassId, evt.StudentId));
    }
}
