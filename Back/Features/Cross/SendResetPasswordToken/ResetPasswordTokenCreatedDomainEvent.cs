namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public record ResetPasswordTokenCreatedDomainEvent(Guid UserId) : IDomainEvent;

public class ResetPasswordTokenCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<ResetPasswordTokenCreatedDomainEvent>
{
    public async Task Handle(ResetPasswordTokenCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(new SendResetPasswordEmailTask(evt.UserId));
    }
}
