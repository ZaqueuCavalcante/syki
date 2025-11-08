using Exato.Back.Emails;

namespace Exato.Back.Features.Cross.SendResetPasswordToken;

public record SendResetPasswordEmailCommand(Guid UserId) : ICommand;

public class SendResetPasswordEmailCommandHandler(BackDbContext ctx, IEmailService emailService) : ICommandHandler<SendResetPasswordEmailCommand>
{
    public async Task Handle(Guid commandId, SendResetPasswordEmailCommand command)
    {
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == command.UserId);

        var reset = await ctx.ExatoResetPasswordTokens.AsNoTracking()
            .OrderByDescending(r => r.CreatedAt)
            .FirstAsync(r => r.UserId == user.Id);

        await emailService.SendResetPasswordEmail(user.Email, reset.Id.ToString());
    }
}
