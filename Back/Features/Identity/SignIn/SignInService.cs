using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Syki.Back.Features.Identity.SignIn;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Cross.SignIn;

public class SignInService(
    SykiDbContext ctx,
    AuthSettings settings,
    IHttpContextAccessor httpCtx,
    UserManager<SykiUser> userManager) : ICrossService
{
    public async Task<SignInOut> SignIn(string email)
    {
        var user = (await userManager.FindByEmailAsync(email))!;

        var claims = new List<Claim>
        {
            new(Claims.Jti, Guid.NewGuid().ToString()),
            new(Claims.UserId, user.Id.ToString()),
            new(Claims.UserName, user.Name),
            new(Claims.UserEmail, user.Email!),
            new(Claims.InstitutionId, user.InstitutionId.ToString()),
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

    private async Task<List<Claim>> GetDbClaims(Guid userId, string role)
    {
        if (role.ToEnum<UserRole>() is UserRole.Student)
        {
            var courseOfferingId = await ctx.Students.Where(a => a.Id == userId)
                .Select(a => a.CourseOfferingId).FirstAsync();
            var courseCurriculumId = await ctx.CourseOfferings.Where(o => o.Id == courseOfferingId)
                .Select(o => o.CourseCurriculumId).FirstAsync();

            return [ new(Claims.CourseCurriculumId, courseCurriculumId.ToString()) ];
        }

        return [];
    }
}
