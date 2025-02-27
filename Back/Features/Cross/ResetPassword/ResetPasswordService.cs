using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.ResetPassword;

public class ResetPasswordService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Reset(ResetPasswordIn body)
    {
        _ = Guid.TryParse(body.Token, out var id);
        var reset = await ctx.ResetPasswordTokens.FirstOrDefaultAsync(r => r.Id == id);
        if (reset == null) return new UserNotFound();

        var user = await userManager.FindByIdAsync(reset!.UserId.ToString());

        var result = await userManager.ResetPasswordAsync(user!, reset.Token, body.Password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken")) return new InvalidResetToken();
            
            return new WeakPassword();
        }

        reset.Use();
        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
