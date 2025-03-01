using Syki.Back.Emails;

namespace Syki.Back.Features.Cross.CreatePendingUserRegister;

[CommandDescription("Enviar email de confirmação de cadastro")]
public record SendUserRegisterEmailConfirmationCommand(Guid UserRegisterId) : ICommand;

public class SendUserRegisterEmailConfirmationCommandHandler(SykiDbContext ctx, IEmailsService emailsService) : ICommandHandler<SendUserRegisterEmailConfirmationCommand>
{
    public async Task Handle(SendUserRegisterEmailConfirmationCommand command)
    {
        var register = await ctx.UserRegisters.AsNoTracking().FirstAsync(d => d.Id == command.UserRegisterId);

        await emailsService.SendUserRegisterEmailConfirmation(register.Email, register.Id.ToString());
    }
}
