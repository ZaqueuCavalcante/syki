using Syki.Dtos;
using System.Text;
using Syki.Back.Settings;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SykiUser = Syki.Back.Domain.SykiUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Services;

public class AuthService
{
    private readonly AuthSettings _settings;
    private readonly UserManager<SykiUser> _userManager;

    public AuthService(
        AuthSettings settings,
        UserManager<SykiUser> userManager
    ) {
        _settings = settings;
        _userManager = userManager;
    }

    public async Task Register(RegisterIn body)
    {
        var user = new SykiUser
        {
            FaculdadeId = body.Faculdade,
            Name = body.Name,
            UserName = body.Email,
            Email = body.Email,
        };

        await _userManager.CreateAsync(user, body.Password);

        if (body.Role is Academico or Professor or Aluno)
        {
            await _userManager.AddToRoleAsync(user, body.Role);
        }
    }

    public async Task<string> GenerateAccessToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var facul = user!.FaculdadeId != null ? user.FaculdadeId.ToString()! : Guid.Empty.ToString();

        var claims = new List<Claim>
        {
            new ("jti", Guid.NewGuid().ToString()),
            new ("sub", user!.Id.ToString()),
            new ("name", user.Name),
            new ("email", user.Email!),
            new ("faculdade", facul),
        };

        var roleNames = await _userManager.GetRolesAsync(user);
        foreach (var role in roleNames)
        {
            claims.Add(new Claim("role", role));
        }

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