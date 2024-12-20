namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

public record PendingUserRegisterCreatedDomainEvent(Guid UserRegisterId) : IDomainEvent;

public class PendingUserRegisterCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<PendingUserRegisterCreatedDomainEvent>
{
    public async Task Handle(PendingUserRegisterCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(new SendUserRegisterEmailConfirmationTask(evt.UserRegisterId));
    }
}
