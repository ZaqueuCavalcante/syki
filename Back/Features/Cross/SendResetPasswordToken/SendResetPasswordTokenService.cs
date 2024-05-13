using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task Send(SendResetPasswordTokenIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null)
            Throw.DE019.Now();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, token);
        ctx.Add(reset);
        ctx.Add(SykiTask.SendResetPasswordEmail(user.Id));

        await ctx.SaveChangesAsync();
    }
}
