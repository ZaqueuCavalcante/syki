using Syki.Back.Features.Cross.CreateUser;
using Syki.Back.Features.Cross.GenerateJWT;

namespace Syki.Back.Features.Cross.Login;

public class LoginService(GenerateJWTService service, SignInManager<SykiUser> signInManager) : ICrossService
{
    public async Task<OneOf<LoginOut, SykiError>> Login(LoginIn data)
    {
        var result = await signInManager.PasswordSignInAsync(
            userName: data.Email,
            password: data.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (!result.Succeeded && !result.RequiresTwoFactor)
            return new LoginWrongEmailOrPassword();

        if (result.RequiresTwoFactor)
            return new LoginRequiresTwoFactor();

        var jwt = await service.Generate(data.Email);

        return new LoginOut { AccessToken = jwt };
    }
}
