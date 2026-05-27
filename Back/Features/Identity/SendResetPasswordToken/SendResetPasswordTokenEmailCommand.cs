using Syki.Back.Emails;

namespace Syki.Back.Features.Identity.SendResetPasswordToken;

public record SendResetPasswordTokenEmailCommand(string Email, Guid ResetId) : ICommand;

public class SendResetPasswordTokenEmailCommandHandler(IEmailsService emailService) : ICommandHandler<SendResetPasswordTokenEmailCommand>
{
    public async Task Handle(int commandId, SendResetPasswordTokenEmailCommand command)
    {
        await emailService.SendResetPasswordEmail(command.Email, command.ResetId.ToString());
    }
}
