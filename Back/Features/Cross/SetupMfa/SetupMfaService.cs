using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.SetupMfa;

public class SetupMfaService(UserManager<SykiUser> userManager)
{
    public async Task Setup(Guid userId, string token)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId);

        var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
        var ok = await userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token.OnlyNumbers());

        if (!ok) Throw.DE027.Now();
        
        await userManager.SetTwoFactorEnabledAsync(user, true);
    }
}
