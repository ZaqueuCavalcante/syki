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
        ctx.AddRange(SykiDefaultRoles.Director, SykiDefaultRoles.Teacher);
        await ctx.SaveChangesAsync();
    }
}
