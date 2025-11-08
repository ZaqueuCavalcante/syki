using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Cross.SendResetPasswordToken;

namespace Exato.Back.Features.Cross.SendResetPasswordToken;

public class SendResetPasswordTokenService(BackDbContext ctx, UserManager<ExatoUser> userManager) : ICrossService
{
    public async Task<OneOf<ExatoSuccess, ExatoError>> Send(SendResetPasswordTokenIn data)
    {
        var user = await userManager.FindByEmailAsync(data.Email);
        if (user == null) return new UserNotFound();

        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPasswordToken(user.Id, user.OrganizationId, token);
        await ctx.SaveChangesAsync(reset);

        return new ExatoSuccess();
    }
}
