namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[DomainEventDescription("Registro de usuário criado")]
public record PendingUserRegisterCreatedDomainEvent(Guid UserRegisterId) : IDomainEvent;

public class PendingUserRegisterCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<PendingUserRegisterCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, PendingUserRegisterCreatedDomainEvent evt)
    {
        await ctx.SaveCommandsAsync(institutionId, eventId, new SendUserRegisterEmailConfirmationCommand(evt.UserRegisterId));
    }
}
