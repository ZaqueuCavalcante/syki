using Syki.Back.CreateUser;

namespace Syki.Back.SendResetPasswordToken;

public class SendResetPasswordEmailService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task Send(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            Throw.DE019.Now();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, token);
        ctx.Add(reset);
        await ctx.SaveChangesAsync();

        ctx.Add(SykiTask.SendResetPasswordEmail(userId));
    }
}
