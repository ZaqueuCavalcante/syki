using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Cross.ResetPassword;

namespace Exato.Back.Features.Cross.ResetPassword;

public class ResetPasswordService(BackDbContext ctx, UserManager<ExatoUser> userManager) : ICrossService
{
    public async Task<OneOf<ExatoSuccess, ExatoError>> Reset(ResetPasswordIn body)
    {
        _ = Guid.TryParse(body.Token, out var id);
        var reset = await ctx.ExatoResetPasswordTokens.FirstOrDefaultAsync(r => r.Id == id);
        if (reset == null) return new UserNotFound();

        var user = await userManager.FindByIdAsync(reset!.UserId.ToString());

        var result = await userManager.ResetPasswordAsync(user!, reset.Token, body.Password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken")) return new InvalidResetPasswordToken();
            
            return new WeakPassword();
        }

        reset.Use();
        await ctx.SaveChangesAsync();

        return new ExatoSuccess();
    }
}
