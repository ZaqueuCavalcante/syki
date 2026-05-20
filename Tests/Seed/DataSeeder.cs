using Syki.Back.Auth.Roles;

namespace Syki.Tests.Seed;

public class DataSeeder(SykiDbContext ctx)
{
    public async Task Run()
    {
        await SeedDefaultRoles();
    }

    private async Task SeedDefaultRoles()
    {
        ctx.AddRange(SykiDefaultRoles.Director);
        await ctx.SaveChangesAsync();
        var defaultRoles = await ctx.Roles.Where(x => x.OwnerId == null).ToListAsync();
        SykiDefaultRoles.DirectorId = defaultRoles.First(x => x.NormalizedName == SykiDefaultRoles.Director.NormalizedName).Id;
    }
}
