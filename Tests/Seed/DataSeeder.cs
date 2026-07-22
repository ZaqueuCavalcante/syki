namespace Estud.Tests.Seed;

public class DataSeeder(EstudDbContext ctx)
{
    public async Task Run()
    {
        await ctx.SaveChangesAsync();
    }
}
