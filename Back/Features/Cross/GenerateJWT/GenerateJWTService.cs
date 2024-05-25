using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.GenerateJWT;

public class GenerateJWTService(AuthSettings settings, UserManager<SykiUser> userManager)
{
    public async Task<string> Generate(string email)
    {
        var user = (await userManager.FindByEmailAsync(email))!;
        var role = (await userManager.GetRolesAsync(user))[0];

        var claims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("sub", user.Id.ToString()),
            new("role", role),
            new("name", user.Name),
            new("email", user.Email!),
            new("institution", user.InstitutionId.ToString()),
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
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
