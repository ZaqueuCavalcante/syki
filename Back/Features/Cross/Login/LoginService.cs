using Syki.Back.GenerateJWT;

namespace Syki.Back.Features.Cross.Login;

public class LoginService(GenerateJWTService service, SignInManager<SykiUser> signInManager)
{
    public async Task<LoginOut> Login(LoginIn data)
    {
        var result = await signInManager.PasswordSignInAsync(
            userName: data.Email,
            password: data.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (!result.Succeeded && !result.RequiresTwoFactor)
            return new LoginOut { WrongEmailOrPassword = true };

        if (result.RequiresTwoFactor)
            return new LoginOut { RequiresTwoFactor = true };

        var jwt = await service.Generate(data.Email);

        return new LoginOut { AccessToken = jwt };
    }
}
