using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.GenerateJWT;

namespace Syki.Back.Features.Cross.LoginMfa;

public class LoginMfaService(GenerateJWTService service, SignInManager<SykiUser> signInManager) : ICrossService
{
    public async Task<OneOf<LoginMfaOut, SykiError>> LoginMfa(LoginMfaIn data)
    {
        var token = data.Token!.OnlyNumbers();
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded)
            return new LoginWrongMfaToken();

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        var jwt = await service.Generate(user!.Email!);

        return new LoginMfaOut { AccessToken = jwt };
    }
}
