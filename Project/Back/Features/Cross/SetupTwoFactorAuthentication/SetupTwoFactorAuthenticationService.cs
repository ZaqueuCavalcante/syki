using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.SetupTwoFactorAuthentication;

public class SetupTwoFactorAuthenticationService(UserManager<ExatoUser> userManager) : ICrossService
{
    public async Task<OneOf<ExatoSuccess, ExatoError>> Setup(Guid userId, string token)
    {
        var user = await userManager.Users.FirstAsync(u => u.Id == userId);

        var tokenProvider = userManager.Options.Tokens.AuthenticatorTokenProvider;
        var valid = await userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token.OnlyNumbers());

        if (!valid) return Invalid2faToken.I;
        
        await userManager.SetTwoFactorEnabledAsync(user, true);

        return new ExatoSuccess();
    }
}
