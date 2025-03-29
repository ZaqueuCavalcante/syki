namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[DomainEvent(nameof(UserRegister), "Registro de usuário criado")]
public record PendingUserRegisterCreatedDomainEvent(Guid UserRegisterId) : IDomainEvent;

public class PendingUserRegisterCreatedDomainEventHandler(SykiDbContext ctx) : IDomainEventHandler<PendingUserRegisterCreatedDomainEvent>
{
    public async Task Handle(Guid institutionId, Guid eventId, PendingUserRegisterCreatedDomainEvent evt)
    {
        ctx.AddCommand(institutionId, new SendUserRegisterEmailConfirmationCommand(evt.UserRegisterId), eventId: eventId);
        await Task.CompletedTask;
    }
}
