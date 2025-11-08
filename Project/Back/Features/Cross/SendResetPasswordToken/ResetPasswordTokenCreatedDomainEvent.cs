namespace Exato.Back.Features.Cross.SendResetPasswordToken;

public record ResetPasswordTokenCreatedDomainEvent(Guid UserId) : IDomainEvent;

public class ResetPasswordTokenCreatedDomainEventHandler(BackDbContext ctx) : IDomainEventHandler<ResetPasswordTokenCreatedDomainEvent>
{
    public async Task Handle(int organizationId, Guid eventId, ResetPasswordTokenCreatedDomainEvent evt)
    {
        ctx.AddCommand(organizationId, new SendResetPasswordEmailCommand(evt.UserId), eventId: eventId);
        await Task.CompletedTask;
    }
}
