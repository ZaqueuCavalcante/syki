using Syki.Back.Features.Cross.SignIn;

namespace Syki.Back.Features.Academic.CrossLogin;

public class CrossLoginService(SykiDbContext ctx, SignInService service) : ISykiService
{
    public async Task<OneOf<CrossLoginOut, SykiError>> Login(Guid institutionId, CrossLoginIn data)
    {
        var targetUser = await ctx.Users.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId && c.Id == data.TargetUserId)
            .FirstOrDefaultAsync();

        if (targetUser == null) return new UserNotFound();

        await service.SignIn(targetUser.Email);

        return new CrossLoginOut();
    }
}
