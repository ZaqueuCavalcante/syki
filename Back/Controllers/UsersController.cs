using Syki.Dtos;
using System.Text;
using Syki.Back.Database;
using Syki.Back.Settings;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly SykiDbContext _ctx;
    private readonly AuthSettings _settings;

    public UsersController(
        SykiDbContext ctx,
        AuthSettings settings
    ) {
        _ctx = ctx;
        _settings = settings;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginIn body)
    {
        await Task.Delay(1);

        var user = _ctx.SykiUsers.First(u => u.Email == body.Email);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim("faculdade", user.FaculdadeId.ToString()),
            //new Claim("role", user.Role),
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
        var jwt = tokenHandler.WriteToken(token);

        return Ok(new LoginOut { AccessToken = jwt });
    }
}
