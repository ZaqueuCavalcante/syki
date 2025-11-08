using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Exato.Shared.Features.Cross.GenerateJWT;
using Exato.Back.Features.Cross.CreateExatoUser;

namespace Exato.Back.Features.Cross.GenerateJWT;

public class GenerateJWTService(AuthSettings settings, BackDbContext ctx) : ICrossService
{
    public async Task<GenerateJWTOut> Generate(string email)
    {
        var user = await ctx.Users.FirstAsync(x => x.Email == email);
        var userRole = await ctx.UserRoles.SingleAsync(x => x.UserId == user.Id);
        var role = await ctx.Roles.SingleAsync(x => x.Id == userRole.RoleId);

        var claims = new List<Claim>
        {
            new(Claims.UserId, user.Id.ToString()),
            new(Claims.UserRole, role.Name),
            new(Claims.UserName, user.Name),
            new(Claims.UserEmail, user.Email),
            new(Claims.UserFeatures, role.Features.Serialize()),
            new(Claims.OrganizationId, user.OrganizationId.ToString()),
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

        return new GenerateJWTOut
        {
            Id = user.Id,
            Name = user.Name,
            Role = role.Name,
            Email = user.Email!,
            Features = role.Features,
            JWT = tokenHandler.WriteToken(securityToken),
        };
    }
}
