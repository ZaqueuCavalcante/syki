using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Send(SendResetPasswordTokenIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null) return new UserNotFound();

        // TODO: Locked out?
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        ctx.Add(new ResetPasswordToken(user.Id, token));

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
