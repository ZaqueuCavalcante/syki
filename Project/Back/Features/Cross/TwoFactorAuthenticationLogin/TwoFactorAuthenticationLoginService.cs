using Exato.Back.Features.Cross.GenerateJWT;
using Exato.Shared.Features.Cross.GenerateJWT;
using Exato.Back.Features.Cross.CreateExatoUser;
using Exato.Shared.Features.Cross.TwoFactorAuthenticationLogin;

namespace Exato.Back.Features.Cross.TwoFactorAuthenticationLogin;

public class TwoFactorAuthenticationLoginService(GenerateJWTService service, SignInManager<ExatoUser> signInManager) : ICrossService
{
    public async Task<OneOf<GenerateJWTOut, ExatoError>> Login(TwoFactorAuthenticationLoginIn data)
    {
        var token = data.Token!.OnlyNumbers();
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded)
            return Invalid2faToken.I;

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        return await service.Generate(user.Email);
    }
}
