namespace Syki.Back.Features.Cross.SendResetPasswordToken;

[DomainEvent(nameof(ResetPasswordToken), "Token de redefinição de senha gerado")]
public record ResetPasswordTokenCreatedDomainEvent(Guid UserId) : IDomainEvent;

public class ResetPasswordTokenCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ResetPasswordTokenCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, DomainEventId eventId, ResetPasswordTokenCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new SendResetPasswordEmailCommand(evt.UserId), eventId: eventId);
        await Task.CompletedTask;
    }
}
