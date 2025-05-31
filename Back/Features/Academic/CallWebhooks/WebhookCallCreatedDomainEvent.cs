namespace Syki.Back.Features.Academic.CallWebhooks;

[DomainEvent(nameof(WebhookCall), "Chamada de webhook criada")]
public record WebhookCallCreatedDomainEvent(Guid WebhookCallId) : IDomainEvent;

public class WebhookCallCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<WebhookCallCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, WebhookCallCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new CallWebhookCommand(evt.WebhookCallId), eventId: eventId);

        await Task.CompletedTask;
    }
}
