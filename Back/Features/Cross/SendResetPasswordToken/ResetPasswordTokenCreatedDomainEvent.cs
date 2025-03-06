namespace Syki.Back.Features.Cross.SendResetPasswordToken;

[DomainEventDescription("Token de redefinição de senha gerado")]
public record ResetPasswordTokenCreatedDomainEvent(Guid UserId) : IDomainEvent;

public class ResetPasswordTokenCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ResetPasswordTokenCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, ResetPasswordTokenCreatedDomainEvent evt)
    {
        ctx.AddCommands(institutionId, eventId, new SendResetPasswordEmailCommand(evt.UserId));
        await Task.CompletedTask;
    }
}
