namespace Syki.Back.Features.Academic.CallWebhooks;

[DomainEvent(nameof(WebhookCall), "Chamada de webhook criada")]
public record WebhookCallFailedDomainEvent(Guid WebhookCallId, int DelaySeconds) : IDomainEvent;

public class WebhookCallFailedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<WebhookCallFailedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, WebhookCallFailedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new CallWebhookCommand(evt.WebhookCallId), eventId: eventId, delaySeconds: evt.DelaySeconds);

        await Task.CompletedTask;
    }
}
