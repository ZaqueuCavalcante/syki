using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.GenerateJWT;

namespace Syki.Back.Features.Cross.LoginMfa;

public class LoginMfaService(GenerateJWTService service, SignInManager<SykiUser> signInManager)
{
    public async Task<LoginMfaOut> Login(LoginMfaIn data)
    {
        var token = data.Token!.OnlyNumbers();
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded)
            return new LoginMfaOut { Wrong2FactorCode = true };

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        var jwt = await service.Generate(user!.Email!);

        return new LoginMfaOut { AccessToken = jwt };
    }
}
