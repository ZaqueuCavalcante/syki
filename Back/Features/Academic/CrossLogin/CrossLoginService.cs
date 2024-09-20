using Syki.Back.Features.Cross.GenerateJWT;

namespace Syki.Back.Features.Academic.CrossLogin;

public class CrossLoginService(SykiDbContext ctx, GenerateJWTService service) : IAcademicService
{
    public async Task<OneOf<CrossLoginOut, SykiError>> Login(Guid userId, CrossLoginIn data)
    {
        var institutionId = await ctx.Users.AsNoTracking()
            .Where(c => c.Id == userId)
            .Select(c => c.InstitutionId)
            .FirstAsync();
        
        var targetUser = await ctx.Users.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId && c.Id == data.TargetUserId)
            .FirstOrDefaultAsync();

        if (targetUser == null) return new UserNotFound();

        var token = await service.Generate(targetUser.Email);

        return new CrossLoginOut { AccessToken = token };
    }
}
