using System.Text;
using Estud.Back.Auth.Claims;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Estud.Back.Features.Identity.SignIn;

namespace Estud.Back.Features.Cross.SignIn;

public class SignInService(
    EstudDbContext ctx,
    AuthSettings settings,
    IHttpContextAccessor httpCtx) : IEstudService
{
    public async Task<SignInOut> SignIn(string email)
    {
        var user = await ctx.Users.Where(u => u.Email == email).Select(x => new { x.Id, x.InstitutionId }).FirstAsync();
        var role = await ctx.GetUserRole(user.Id, user.InstitutionId);
        var permissions = role.Permissions.Serialize();

        var claims = new List<Claim>
        {
            new(EstudClaims.UserId, user.Id.ToString()),
            new(EstudClaims.UserPermissions, permissions),
            new(EstudClaims.Jti, Guid.NewGuid().ToString()),
            new(EstudClaims.UserType, role.BaseType.ToInt().ToString()),
            new(EstudClaims.UserInstitutionId, user.InstitutionId.ToString()),
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
            InstitutionId = user.InstitutionId,
        };
    }
}
