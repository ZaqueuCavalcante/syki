namespace Syki.Back.Features.Cross.SendResetPasswordToken;

[DomainEventDescription("Token de redefinição de senha gerado")]
public record ResetPasswordTokenCreatedDomainEvent(Guid UserId) : IDomainEvent;

public class ResetPasswordTokenCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ResetPasswordTokenCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, ResetPasswordTokenCreatedDomainEvent evt)
    {
        await ctx.SaveTasksAsync(eventId, new SendResetPasswordEmailTask(evt.UserId));
    }
}
