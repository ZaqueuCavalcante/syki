namespace Syki.Back.Features.Teacher.CreateClassActivity;

[DomainEvent(nameof(ClassActivity), "Atividade publicada")]
public record ClassActivityCreatedDomainEvent(Guid ClassActivityId) : IDomainEvent;

public class ClassActivityCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ClassActivityCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, DomainEventId eventId, ClassActivityCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new CreateNewClassActivityNotificationCommand(evt.ClassActivityId), eventId: eventId);

        var webhooks = await ctx.Webhooks
            .Where(x => x.InstitutionId == institutionId && x.Events.Contains(WebhookEventType.ClassActivityCreated))
            .Select(x => new { x.Id })
            .ToListAsync();
        foreach (var webhook in webhooks)
        {
            ctx.AddCommand(institutionId, new CreateClassActivityCreatedWebhookCallCommand(eventId, webhook.Id, evt.ClassActivityId), eventId: eventId);
        }
    }
}
