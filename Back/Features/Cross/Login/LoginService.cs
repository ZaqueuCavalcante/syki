using System.IdentityModel.Tokens.Jwt;
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
        var claims = new JwtSecurityToken(jwt).Claims.ToList();

        return new LoginOut
        {
            AccessToken = jwt,
            Name = claims.First(x => x.Type == "name").Value,
            Email = claims.First(x => x.Type == "email").Value,
            Id = Guid.Parse(claims.First(x => x.Type == "sub").Value),
            Role = Enum.Parse<UserRole>(claims.First(x => x.Type == "role").Value),
        };
    }
}
