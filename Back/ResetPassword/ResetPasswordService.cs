using Syki.Back.CreateUser;

namespace Syki.Back.ResetPassword;

public class ResetPasswordService(SykiDbContext ctx, UserManager<SykiUser> userManager)
{
    public async Task Reset(ResetPasswordIn body)
    {
        _ = Guid.TryParse(body.Token, out var id);
        var reset = await ctx.ResetPasswordTokens
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reset == null)
            Throw.DE019.Now();

        var user = await userManager.FindByIdAsync(reset!.UserId.ToString());

        var result = await userManager.ResetPasswordAsync(user!, reset.Token, body.Password);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
                Throw.DE020.Now();
            
            Throw.DE015.Now();
        }

        reset.Use();
        await ctx.SaveChangesAsync();
    }
}
