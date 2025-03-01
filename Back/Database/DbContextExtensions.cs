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

    public static async Task SaveTasksAsync(this SykiDbContext ctx, Guid eventId, Guid institutionId, params ISykiTask[] tasks)
    {
        foreach (var task in tasks)
        {
            ctx.Add(new SykiTask(eventId, institutionId, task));
        }

        await ctx.SaveChangesAsync();
    }
}
