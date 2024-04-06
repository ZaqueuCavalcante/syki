using Syki.Back.Features.Cross.GenerateJWT;

namespace Syki.Back.LoginMfa;

public class LoginMfaService(GenerateJWTService service, SignInManager<SykiUser> signInManager)
{
    public async Task<LoginMfaOut> Login(LoginMfaIn data)
    {
        var token = data.Code!.OnlyNumbers();
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded)
            return new LoginMfaOut { Wrong2FactorCode = true };

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        var jwt = await service.Generate(user!.Email!);

        return new LoginMfaOut { AccessToken = jwt };
    }
}
