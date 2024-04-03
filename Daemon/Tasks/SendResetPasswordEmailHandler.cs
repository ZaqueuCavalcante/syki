using Syki.Back.Tasks;
using Syki.Back.Database;
using Syki.Daemon.Emails;
using Syki.Back.Exceptions;
using Microsoft.EntityFrameworkCore;
using Syki.Back.SendResetPasswordToken;

namespace Syki.Daemon.Tasks;

public class SendResetPasswordEmailHandler(SykiDbContext ctx, IEmailsService emailsService) : ISykiTaskHandler<SendResetPasswordEmail>
{
    public async Task Handle(SendResetPasswordEmail task)
    {
        var user = await ctx.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == task.UserId);
        if (user == null)
            Throw.DE019.Now();

        var reset = await ctx.ResetPasswordTokens
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync(r => r.UserId == user.Id && r.UsedAt == null);

        await emailsService.SendResetPasswordEmail(user.Email, reset.Id.ToString());
    }
}
