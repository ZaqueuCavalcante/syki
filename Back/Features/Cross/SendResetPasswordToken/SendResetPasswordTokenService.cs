using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Send(SendResetPasswordTokenIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null) return new UserNotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, user.InstitutionId, token);
        ctx.Add(reset);
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
