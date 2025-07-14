using System.IdentityModel.Tokens.Jwt;
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
        var claims = new JwtSecurityToken(jwt).Claims.ToList();

        return new LoginMfaOut
        {
            AccessToken = jwt,
            Name = claims.First(x => x.Type == "name").Value,
            Email = claims.First(x => x.Type == "email").Value,
            Id = Guid.Parse(claims.First(x => x.Type == "sub").Value),
            Role = Enum.Parse<UserRole>(claims.First(x => x.Type == "role").Value),
        };
    }
}
