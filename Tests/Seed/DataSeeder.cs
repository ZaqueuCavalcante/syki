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
        ctx.AddRange(
            EstudDefaultRoles.Parent,
            EstudDefaultRoles.Student,
            EstudDefaultRoles.Teacher,
            EstudDefaultRoles.Director
        );
        await ctx.SaveChangesAsync();
    }
}
