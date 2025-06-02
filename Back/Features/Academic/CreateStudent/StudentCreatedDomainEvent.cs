using Syki.Back.Features.Cross.LinkOldNotifications;

namespace Syki.Back.Features.Academic.CreateStudent;

[DomainEvent(nameof(SykiStudent), "Aluno criado")]
public record StudentCreatedDomainEvent(Guid UserId, Guid InstitutionId) : IDomainEvent;

public class StudentCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<StudentCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, StudentCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new LinkOldNotificationsCommand(evt.InstitutionId, evt.UserId), eventId: eventId);
        ctx.AddCommand(institutionId, new SendStudentWelcomeEmailCommand(evt.InstitutionId, evt.UserId), eventId: eventId);

        var webhooks = await ctx.Webhooks
            .Where(x => x.InstitutionId == institutionId && x.Events.Contains(WebhookEventType.StudentCreated))
            .Select(x => new { x.Id })
            .ToListAsync();
        foreach (var webhook in webhooks)
        {
            ctx.AddCommand(institutionId, new CreateStudentCreatedWebhookCallCommand(eventId, webhook.Id, evt.UserId), eventId: eventId);
        }
    }
}
