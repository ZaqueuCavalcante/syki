using Syki.Back.Emails;

namespace Syki.Back.Features.Cross.SendResetPasswordToken;

[CommandDescription("Enviar email de redefinição de senha")]
public record SendResetPasswordEmailCommand(Guid UserId) : ICommand;

public class SendResetPasswordEmailCommandHandler(SykiDbContext ctx, IEmailsService emailsService) : ICommandHandler<SendResetPasswordEmailCommand>
{
    public async Task Handle(CommandId commandId, SendResetPasswordEmailCommand command)
    {
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == command.UserId);

        var reset = await ctx.ResetPasswordTokens.AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .FirstAsync(r => r.UserId == user.Id);

        await emailsService.SendResetPasswordEmail(user.Email, reset.Id.ToString());
    }
}
