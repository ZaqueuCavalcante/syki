namespace Syki.Back.Features.Teacher.AddExamGradeNote;

public record ExamGradeNoteAddedDomainEvent(Guid StudentId, Guid ClassId) : IDomainEvent;

public class ExamGradeNoteAddedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ExamGradeNoteAddedDomainEvent>
{
    public async Task Handle(Guid eventId, ExamGradeNoteAddedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(eventId, new CreateNewExamGradeNoteNotificationTask(evt.StudentId, evt.ClassId));
    }
}
