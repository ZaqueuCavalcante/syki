namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public record ResetPasswordTokenCreatedDomainEvent(Guid UserId) : IDomainEvent;

public class ResetPasswordTokenCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ResetPasswordTokenCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, ResetPasswordTokenCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(eventId, new SendResetPasswordEmailTask(evt.UserId));
    }
}
