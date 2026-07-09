using Estud.Back.Auth.Roles;

namespace Estud.Tests.Seed;

public class DataSeeder(EstudDbContext ctx)
{
    public async Task Run()
    {
        await SeedDefaultRoles();
    }

    private async Task SeedDefaultRoles()
    {
        ctx.AddRange(EstudDefaultRoles.Director, EstudDefaultRoles.Teacher, EstudDefaultRoles.Student);
        await ctx.SaveChangesAsync();
    }
}
