namespace Syki.Back.Features.Teacher.CreateClassActivity;

[DomainEventDescription("Atividade criada")]
public record ClassActivityCreatedDomainEvent(Guid ClassActivityId) : IDomainEvent;

public class ClassActivityCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ClassActivityCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, ClassActivityCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new CreateNewClassActivityNotificationCommand(evt.ClassActivityId), eventId: eventId);
        await Task.CompletedTask;
    }
}
