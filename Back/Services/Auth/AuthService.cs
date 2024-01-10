using Dapper;
using Npgsql;
using Syki.Shared;
using System.Text;
using Syki.Back.Database;
using Syki.Back.Settings;
using Syki.Back.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SykiUser = Syki.Back.Domain.SykiUser;
using static Syki.Back.Configs.AuthorizationConfigs;
using ResetPassword = Syki.Back.Domain.ResetPassword;

namespace Syki.Back.Services;

public class AuthService : IAuthService
{
    private readonly SykiDbContext _ctx;
    private readonly AuthSettings _settings;
    private readonly DatabaseSettings _dbSettings;
    private readonly UserManager<SykiUser> _userManager;

    public AuthService(
        SykiDbContext ctx,
        AuthSettings settings,
        DatabaseSettings dbSettings,
        UserManager<SykiUser> userManager
    ) {
        _ctx = ctx;
        _settings = settings;
        _dbSettings = dbSettings;
        _userManager = userManager;
    }

    public async Task<UserOut> Register(UserIn body)
    {
        if (!(body.Role is Academico or Professor or Aluno))
            throw new DomainException(ExceptionMessages.DE0010);

        var faculdadeOk = await _ctx.Faculdades.AnyAsync(c => c.Id == body.Faculdade);
        if (!faculdadeOk)
            throw new DomainException(ExceptionMessages.DE0011);

        if (!body.Email.IsValidEmail())
            throw new DomainException(ExceptionMessages.DE0013);

        var emailUsed = await _ctx.Users.AnyAsync(u => u.Email == body.Email);
        if (emailUsed)
            throw new DomainException(ExceptionMessages.DE0014);

        var user = new SykiUser
        {
            FaculdadeId = body.Faculdade,
            Name = body.Name,
            UserName = body.Email,
            Email = body.Email,
        };

        var result = await _userManager.CreateAsync(user, body.Password);

        if (!result.Succeeded)
            throw new DomainException(ExceptionMessages.DE0012);

        await _userManager.AddToRoleAsync(user, body.Role);

        await GetResetPasswordToken(user.Id);

        return user.ToOut();
    }

    public async Task<string> GetMfaKey(Guid userId)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        var key = await _userManager.GetAuthenticatorKeyAsync(user);

        if (key == null)
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            key = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        return key!;
    }

    public async Task<MfaSetupOut> SetupMfa(Guid userId, string token)
    {
        var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

        var tokenProvider = _userManager.Options.Tokens.AuthenticatorTokenProvider;
        var ok = await _userManager.VerifyTwoFactorTokenAsync(user, tokenProvider, token.OnlyNumbers());

        if (ok)
        {
            await _userManager.SetTwoFactorEnabledAsync(user, true);
        }

        return new MfaSetupOut { Ok = ok };
    }

    public async Task<ResetPasswordTokenOut> GetResetPasswordToken(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            throw new DomainException(ExceptionMessages.DE0016);

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPassword(user.Id, token);
        _ctx.Add(reset);
        await _ctx.SaveChangesAsync();

        return new ResetPasswordTokenOut { Token = token };
    }

    public async Task<ResetPasswordOut> ResetPassword(ResetPasswordIn body)
    {
        var reset = await _ctx.ResetPasswords
            .FirstOrDefaultAsync(r => r.Token == body.Token);

        if (reset == null)
            throw new DomainException(ExceptionMessages.DE0016);

        var user = await _userManager.FindByIdAsync(reset.UserId.ToString());

        var result = await _userManager.ResetPasswordAsync(user!, body.Token, body.Password);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
                throw new DomainException(ExceptionMessages.DE0017);
            
            throw new DomainException(ExceptionMessages.DE0012);
        }

        reset.Use();
        await _ctx.SaveChangesAsync();

        return new ResetPasswordOut { Ok = true };
    }

    public async Task<string> GenerateAccessToken(string email)
    {
        var user = (await _userManager.FindByEmailAsync(email))!;

        var claims = new List<Claim>
        {
            new("jti", Guid.NewGuid().ToString()),
            new("sub", user.Id.ToString()),
            new("name", user.Name),
            new("email", user.Email!),
            new("faculdade", user.FaculdadeId.ToString()),
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

    public async Task<List<UserOut>> GetAllUsers()
    {
        using var connection = new NpgsqlConnection(_dbSettings.ConnectionString);

        const string sql = @"
            SELECT
                u.id,
                u.name AS nome,
                u.email,
                f.nome AS faculdade,
                STRING_AGG(r.name, ',') AS role
            FROM
                syki.users u
            INNER JOIN
                syki.faculdades f ON f.id = u.faculdade_id
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                syki.roles r ON r.id = ur.role_id
            GROUP BY
                u.id, f.nome
        ";

        var data = await connection.QueryAsync<UserOut>(sql);
        
        return data.ToList();
    }
}
