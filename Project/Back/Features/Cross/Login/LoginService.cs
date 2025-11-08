using Exato.Shared.Features.Cross.Login;
using Exato.Back.Features.Cross.GenerateJWT;
using Exato.Shared.Features.Cross.GenerateJWT;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.Login;

public class LoginService(GenerateJWTService service, SignInManager<ExatoUser> signInManager) : ICrossService
{
    public async Task<OneOf<GenerateJWTOut, ExatoError>> Login(LoginIn data)
    {
        var result = await signInManager.PasswordSignInAsync(
            userName: data.Email,
            password: data.Password,
            isPersistent: false,
            lockoutOnFailure: false
        );

        if (!result.Succeeded && !result.RequiresTwoFactor)
            return LoginWrongEmailOrPassword.I;

        if (result.RequiresTwoFactor)
            return LoginRequiresTwoFactor.I;

        return await service.Generate(data.Email);
    }
}
