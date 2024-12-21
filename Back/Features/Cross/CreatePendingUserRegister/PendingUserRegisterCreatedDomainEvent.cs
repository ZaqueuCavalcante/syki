namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[DomainEventDescription("Registro de usuário criado")]
public record PendingUserRegisterCreatedDomainEvent(Guid UserRegisterId) : IDomainEvent;

public class PendingUserRegisterCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<PendingUserRegisterCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, PendingUserRegisterCreatedDomainEvent evt)
    {
        await ctx.SaveTaskAsync(eventId, new SendUserRegisterEmailConfirmationTask(evt.UserRegisterId));
    }
}
