using System.Text;
using Syki.Back.Auth.Claims;
using System.Security.Claims;
using Syki.Back.Domain.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Syki.Back.Features.Identity.SignIn;

namespace Syki.Back.Features.Cross.SignIn;

public class SignInService(
    AuthSettings settings,
    IHttpContextAccessor httpCtx,
    UserManager<SykiUser> userManager) : ISykiService
{
    public async Task<SignInOut> SignIn(string email)
    {
        var user = (await userManager.FindByEmailAsync(email))!;

        var claims = new List<Claim>
        {
            new(SykiClaims.Jti, Guid.NewGuid().ToString()),
            new(SykiClaims.UserId, user.Id.ToString()),
            new(SykiClaims.InstitutionId, user.InstitutionId.ToString()),
        };

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(settings.SecurityKey);
        var expirationTime = settings.ExpirationTimeInMinutes;
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = settings.Issuer,
            Subject = identityClaims,
            Audience = settings.Audience,
            SigningCredentials = signingCredentials,
            Expires = DateTime.UtcNow.AddMinutes(expirationTime),
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        httpCtx.HttpContext.Response.AppendJWTCookie(tokenHandler.WriteToken(securityToken), settings);

        return new SignInOut
        {
            UserId = user.Id,
            Permissions = [],
            InstitutionId = user.InstitutionId,
        };
    }
}
