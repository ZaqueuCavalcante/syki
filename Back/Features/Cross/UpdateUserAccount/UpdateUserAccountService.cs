namespace Syki.Back.Features.Cross.UpdateUserAccount;

public class UpdateUserAccountService(SykiDbContext ctx) : ICrossService
{
    public async Task<OneOf<SykiSuccess, SykiError>> Update(Guid userId, UpdateUserAccountIn data)
    {
        var user = await ctx.Users.FirstAsync(u => u.Id == userId);

        user.Update(data.Name, data.ProfilePhoto);

        await ctx.SaveChangesAsync();

        return new SykiSuccess();
    }
}
