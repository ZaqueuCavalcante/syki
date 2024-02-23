using System.Text;
using Syki.Back.Settings;
using Syki.Back.CreateUser;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Syki.Back.GenerateJWT;

public class GenerateJWTService
{
    private readonly AuthSettings _settings;
    private readonly UserManager<SykiUser> _userManager;
    public GenerateJWTService(
        AuthSettings settings,
        UserManager<SykiUser> userManager
    ) {
        _settings = settings;
        _userManager = userManager;
    }

    public async Task<string> Generate(string email)
    {
        var user = (await _userManager.FindByEmailAsync(email))!;
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("sub", user.Id.ToString()),
            new("role", roles[0]),
            new("name", user.Name),
            new("email", user.Email!),
            new("faculdade", user.InstitutionId.ToString()),
        };

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        var key = Encoding.ASCII.GetBytes(_settings.SecurityKey);
        var expirationTime = _settings.ExpirationTimeInMinutes;
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        );

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _settings.Issuer,
            Audience = _settings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(expirationTime),
            SigningCredentials = signingCredentials,
            Subject = identityClaims
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
