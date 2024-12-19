using Syki.Back.Emails;

namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public record SendResetPasswordEmailTask(Guid UserId) : ISykiTask;

public class SendResetPasswordEmailTaskHandler(SykiDbContext ctx, IEmailsService emailsService) : ISykiTaskHandler<SendResetPasswordEmailTask>
{
    public async Task Handle(SendResetPasswordEmailTask task)
    {
        var user = await ctx.Users.AsNoTracking().FirstAsync(u => u.Id == task.UserId);

        var reset = await ctx.ResetPasswordTokens
            .OrderByDescending(r => r.CreatedAt)
            .FirstAsync(r => r.UserId == user.Id);

        await emailsService.SendResetPasswordEmail(user.Email, reset.Id.ToString());
    }
}
