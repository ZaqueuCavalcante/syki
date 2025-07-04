using Microsoft.EntityFrameworkCore.Metadata;
using Syki.Back.Features.Academic.CreateCourse;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

namespace Syki.Back.Database;

public static class DbContextExtensions
{
    public static void ResetDevDb(this SykiDbContext ctx)
    {
        if (!Env.IsDevelopment()) return;

        ctx.Database.EnsureDeleted();
        ctx.Database.Migrate();
    }

    public static async Task ResetTestDbAsync(this SykiDbContext ctx)
    {
        if (!Env.IsTesting()) return;

        await ctx.Database.EnsureDeletedAsync();
        await ctx.Database.MigrateAsync();
    }

    public static bool HasMissingMigration(this SykiDbContext context)
    {
        var modelDiffer = context.GetService<IMigrationsModelDiffer>();

        var migrationsAssembly = context.GetService<IMigrationsAssembly>();
        var modelInitializer = context.GetService<IModelRuntimeInitializer>();

        var snapshotModel = migrationsAssembly.ModelSnapshot?.Model;
        if (snapshotModel is IMutableModel mutableModel)
            snapshotModel = mutableModel.FinalizeModel();
        if (snapshotModel is not null)
            snapshotModel = modelInitializer.Initialize(snapshotModel);

        var designTimeModel = context.GetService<IDesignTimeModel>();

        return modelDiffer.HasDifferences(
            snapshotModel?.GetRelationalModel(),
            designTimeModel.Model.GetRelationalModel());
    }

    public static async Task<bool> AcademicPeriodExists(this SykiDbContext ctx, Guid institutionId, string id)
    {
        return await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Id == id);
    }

    public static async Task<List<Course>> GetCourses(this SykiDbContext ctx, Guid institutionId)
    {
        return await ctx.Courses.AsNoTracking()
            .Where(c => c.InstitutionId == institutionId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public static Command AddCommand(
        this DbContext ctx,
        Guid institutionId,
        ICommand command,
        DomainEventId? eventId = null,
        CommandId? parentId = null,
        CommandId? originalId = null,
        CommandBatchId? batchId = null,
        int? delaySeconds = null
    ) {
        var activityId = Activity.Current?.Id;

        return ctx.Add(
            new Command(
                institutionId,
                command,
                eventId: eventId,
                parentId: parentId,
                originalId: originalId,
                batchId: batchId,
                delaySeconds: delaySeconds,
                activityId: activityId
            )
        ).Entity;
    }

    public static async Task<int> SaveChangesAsync<TEntity>(this SykiDbContext ctx, TEntity entity)
    {
        ctx.Add(entity);
        return await ctx.SaveChangesAsync();
    }
}
