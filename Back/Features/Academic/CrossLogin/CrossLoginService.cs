using System.IdentityModel.Tokens.Jwt;
using Syki.Back.Features.Cross.GenerateJWT;

namespace Syki.Back.Features.Academic.CrossLogin;

public class CrossLoginService(SykiDbContext ctx, GenerateJWTService service) : IAcademicService
{
    public async Task<OneOf<CrossLoginOut, SykiError>> Login(Guid institutionId, CrossLoginIn data)
    {
        var targetUser = await ctx.Users.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId && c.Id == data.TargetUserId)
            .FirstOrDefaultAsync();

        if (targetUser == null) return new UserNotFound();

        var jwt = await service.Generate(targetUser.Email);
        var claims = new JwtSecurityToken(jwt).Claims.ToList();

        return new CrossLoginOut
        {
            AccessToken = jwt,
            Name = claims.First(x => x.Type == "name").Value,
            Email = claims.First(x => x.Type == "email").Value,
            Id = Guid.Parse(claims.First(x => x.Type == "sub").Value),
            Role = Enum.Parse<UserRole>(claims.First(x => x.Type == "role").Value),
        };
    }
}
