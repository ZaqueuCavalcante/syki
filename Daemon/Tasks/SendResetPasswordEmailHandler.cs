using Syki.Back.Emails;
using Syki.Back.Features.Cross.SendResetPasswordToken;

namespace Syki.Daemon.Tasks;

public class SendResetPasswordEmailHandler(SykiDbContext ctx, IEmailsService emailsService) : ISykiTaskHandler<SendResetPasswordEmail>
{
    public async Task Handle(SendResetPasswordEmail task)
    {
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == task.UserId);

        var reset = await ctx.ResetPasswordTokens
            .OrderByDescending(r => r.CreatedAt)
            .FirstAsync(r => r.UserId == user.Id);

        await emailsService.SendResetPasswordEmail(user.Email, reset.Id.ToString());
    }
}
