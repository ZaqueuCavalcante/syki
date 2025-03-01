namespace Syki.Back.Database;

public static class DbContextExtensions
{
    public static void ResetDb(this SykiDbContext ctx)
    {
        if (Env.IsDevelopment())
        {
            ctx.Database.EnsureDeleted();
            ctx.Database.EnsureCreated();
        }
    }

    public static async Task ResetDbAsync(this SykiDbContext ctx)
    {
        if (Env.IsTesting())
        {
            await ctx.Database.EnsureDeletedAsync();
            await ctx.Database.EnsureCreatedAsync();
        }
    }

    public static void MigrateDb(this SykiDbContext ctx)
    {
        if (!Env.IsTesting())
        {
            ctx.Database.Migrate();
        }
    }

    public static async Task<bool> AcademicPeriodExists(this SykiDbContext ctx, Guid institutionId, string id)
    {
        return await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Id == id);
    }

    public static async Task SaveCommandsAsync(this SykiDbContext ctx, Guid eventId, Guid institutionId, params ICommand[] commands)
    {
        foreach (var command in commands)
        {
            ctx.Add(new Command(eventId, institutionId, command));
        }

        await ctx.SaveChangesAsync();
    }
}
