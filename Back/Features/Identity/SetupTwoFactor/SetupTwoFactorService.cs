using Syki.Back.Domain.Identity;

namespace Syki.Back.Features.Identity.SetupTwoFactor;

public class SetupTwoFactorService(SykiDbContext ctx, UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Setup(string token)
    {
        var webUser = await userManager.Users.FirstAsync(u => u.Id == ctx.RequestUser.Id);

        var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
        var valid = await userManager.VerifyTwoFactorTokenAsync(webUser, tokenProvider, token.OnlyNumbers());
        if (!valid) return InvalidTwoFactorToken.I;

        await userManager.SetTwoFactorEnabledAsync(webUser, true);

        return SykiSuccess.I;
    }
}
