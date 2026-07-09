using Estud.Back.Emails;

namespace Estud.Back.Features.Identity.SendResetPasswordToken;

[CommandDescription("Envia link de redefinição de senha por e-mail.")]
public record SendResetPasswordTokenEmailCommand(string Email, Guid ResetId) : ICommand;

public class SendResetPasswordTokenEmailCommandHandler(IEmailsService emailService) : ICommandHandler<SendResetPasswordTokenEmailCommand>
{
    public async Task Handle(int commandId, SendResetPasswordTokenEmailCommand command)
    {
        await emailService.SendResetPasswordEmail(command.Email, command.ResetId.ToString());
    }
}
