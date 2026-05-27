using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Identity.SendResetPasswordToken;

public class SendResetPasswordTokenService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Send(SendResetPasswordTokenIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null) return SykiSuccess.I; // Don't reveal if the email is registered or not

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, user.InstitutionId, token);
        ctx.AddCommand(user.InstitutionId, new SendResetPasswordTokenEmailCommand(user.Email, reset.Id), maxRetries: 1);
        await ctx.SaveChangesAsync(reset);

        return SykiSuccess.I;
    }
}
