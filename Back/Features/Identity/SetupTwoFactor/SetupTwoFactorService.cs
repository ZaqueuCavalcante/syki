using Estud.Back.Domain.Identity;

namespace Estud.Back.Features.Identity.SetupTwoFactor;

public class SetupTwoFactorService(EstudDbContext ctx, UserManager<EstudUser> userManager) : IEstudService
{
    public async Task<OneOf<EstudSuccess, EstudError>> Setup(string token)
    {
        var webUser = await userManager.Users.FirstAsync(u => u.Id == ctx.RequestUser.Id);

        var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
        var valid = await userManager.VerifyTwoFactorTokenAsync(webUser, tokenProvider, token.OnlyNumbers());
        if (!valid) return InvalidTwoFactorToken.I;

        await userManager.SetTwoFactorEnabledAsync(webUser, true);

        return EstudSuccess.I;
    }
}
