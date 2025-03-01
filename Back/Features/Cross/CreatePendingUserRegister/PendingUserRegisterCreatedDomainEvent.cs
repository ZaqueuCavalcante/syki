namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[DomainEventDescription("Registro de usuário criado")]
public record PendingUserRegisterCreatedDomainEvent(Guid UserRegisterId) : IDomainEvent;

public class PendingUserRegisterCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<PendingUserRegisterCreatedDomainEvent>
{
    public async Task Handle(Guid eventId, Guid institutionId, PendingUserRegisterCreatedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(eventId, institutionId, new SendUserRegisterEmailConfirmationCommand(evt.UserRegisterId));
    }
}
