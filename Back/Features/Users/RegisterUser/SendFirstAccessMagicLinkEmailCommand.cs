using Syki.Back.Emails;

namespace Syki.Back.Features.Users.RegisterUser;

[CommandDescription("Envia link de acesso inicial por e-mail.")]
public record SendFirstAccessMagicLinkEmailCommand(string Email, Guid MagicLinkId) : ICommand;

public class SendFirstAccessMagicLinkEmailCommandHandler(IEmailsService emailService) : ICommandHandler<SendFirstAccessMagicLinkEmailCommand>
{
    public async Task Handle(int commandId, SendFirstAccessMagicLinkEmailCommand command)
    {
        await emailService.SendFirstAccessMagicLinkEmail(command.Email, command.MagicLinkId.ToString());
    }
}
