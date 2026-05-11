using Syki.Back.Features.Cross.SignIn;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.LoginMfa;

public class LoginMfaService(SignInService service, SignInManager<SykiUser> signInManager) : ISykiService
{
    public async Task<OneOf<LoginMfaOut, SykiError>> LoginMfa(LoginMfaIn data)
    {
        var token = data.Token!.OnlyNumbers();
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(token, false, false);

        if (!result.Succeeded) return new LoginWrongMfaToken();

        var user = await signInManager.GetTwoFactorAuthenticationUserAsync();

        await service.SignIn(user!.Email!);

        return new LoginMfaOut();
    }
}
