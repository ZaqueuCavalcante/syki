using Estud.Back.Domain.Identity;

namespace Estud.Back.Features.Identity.ResetPassword;

public class ResetPasswordService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Reset(ResetPasswordIn body)
    {
        _ = Guid.TryParse(body.Token, out var id);
        var reset = await ctx.ResetPasswordTokens.FirstOrDefaultAsync(r => r.Id == id);
        if (reset == null || reset.IsUsed() || reset.IsExpired())
        {
            return InvalidResetPasswordToken.I;
        }

        var user = await userManager.FindByIdAsync(reset.UserId.ToString());

        var result = await userManager.ResetPasswordAsync(user!, reset.Token, body.Password);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken")) return InvalidResetPasswordToken.I;
            return WeakPassword.I;
        }

        reset.Use();
        user!.EmailConfirmed = true;
        await ctx.SaveChangesAsync();

        return EstudSuccess.I;
    }
}
