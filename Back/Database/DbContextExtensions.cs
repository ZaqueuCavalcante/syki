using Microsoft.EntityFrameworkCore.Storage;

namespace Syki.Back.Database;

public static class DbContextExtensions
{
    public static void ResetDb(this SykiDbContext ctx)
    {
        if (!Env.IsTesting())
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

    public static async Task<IDbContextTransaction> BeginTransactionAsync(this SykiDbContext ctx)
    {
        if (ctx.Database.CurrentTransaction == null)
        {
            return await ctx.Database.BeginTransactionAsync();
        }
        return ctx.Database.CurrentTransaction;
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

    public static void AddCommand(this DbContext ctx, Guid institutionId, ICommand command, Guid? eventId = null, Guid? batchId = null)
    {
        ctx.Add(
            new Command(institutionId, command, eventId, batchId)
        );
    }

    public static void AddCommands(this SykiDbContext ctx, Guid institutionId, Guid eventId, params ICommand[] commands)
    {
        foreach (var command in commands)
        {
            ctx.Add(new Command(institutionId, command, eventId, null));
        }
    }
}
