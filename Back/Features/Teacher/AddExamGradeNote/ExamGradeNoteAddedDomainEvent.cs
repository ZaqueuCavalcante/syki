namespace Syki.Back.Features.Teacher.AddExamGradeNote;

[DomainEventDescription("Nota adicionada")]
public record ExamGradeNoteAddedDomainEvent(Guid StudentId, Guid ClassId) : IDomainEvent;

public class ExamGradeNoteAddedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ExamGradeNoteAddedDomainEvent>
{
    public async Task Handle(Guid eventId, ExamGradeNoteAddedDomainEvent evt)
    {
        await ctx.SaveTasksAsync(eventId, new CreateNewExamGradeNoteNotificationTask(evt.StudentId, evt.ClassId));
    }
}
