using Dapper;
using Npgsql;
using Syki.Shared;
using System.Text;
using Syki.Back.Tasks;
using Syki.Back.Database;
using Syki.Back.Settings;
using Syki.Back.Exceptions;
using Syki.Back.CreateUser;
using System.Security.Claims;
using Syki.Shared.CreateUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

    public async Task<CreateUserOut> RegisterUser(CreateUserIn body)
    {
        if (!(body.Role is Academico or Professor or Aluno))
            Throw.DE013.Now();

        var faculdadeOk = await _ctx.Institutions.AnyAsync(c => c.Id == body.InstitutionId);
        if (!faculdadeOk)
            Throw.DE014.Now();

        if (!body.Email.IsValidEmail())
            Throw.DE016.Now();

        var emailUsed = await _ctx.Users.AnyAsync(u => u.Email == body.Email);
        if (emailUsed)
            Throw.DE017.Now();

        var user = new SykiUser(body.InstitutionId, body.Name, body.Email);

        var result = await _userManager.CreateAsync(user, body.Password);
        if (!result.Succeeded)
            Throw.DE015.Now();

        await _userManager.AddToRoleAsync(user, body.Role);

        return user.ToOut();
    }

    public async Task<CreateUserOut> Register(CreateUserIn body)
    {
        var user = await RegisterUser(body);

        await GenerateResetPasswordToken(user.Id);

        var task = new SykiTask(new SendResetPasswordEmail { UserId = user.Id });
        _ctx.Add(task);
        await _ctx.SaveChangesAsync();

        return user;
    }

    public async Task GenerateResetPasswordToken(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
            Throw.DE019.Now();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var reset = new ResetPassword(user.Id, token);
        _ctx.Add(reset);
        await _ctx.SaveChangesAsync();
    }

    public async Task<ResetPasswordOut> ResetPassword(ResetPasswordIn body)
    {
        _ = Guid.TryParse(body.Token, out var id);
        var reset = await _ctx.ResetPasswords
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reset == null)
            Throw.DE019.Now();

        var user = await _userManager.FindByIdAsync(reset!.UserId.ToString());

        var result = await _userManager.ResetPasswordAsync(user!, reset.Token, body.Password);

        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code == "InvalidToken"))
                Throw.DE020.Now();
            
            Throw.DE015.Now();
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
            new("faculdade", user.InstitutionId.ToString()),
        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.Add(new Claim("role", roles[0]));

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

    public async Task<List<CreateUserOut>> GetAllUsers()
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
                syki.faculdades f ON f.id = u.institution_id
            INNER JOIN
                syki.user_roles ur ON ur.user_id = u.id
            INNER JOIN
                syki.roles r ON r.id = ur.role_id
            GROUP BY
                u.id, f.nome
        ";

        var data = await connection.QueryAsync<CreateUserOut>(sql);
        
        return data.ToList();
    }
}
