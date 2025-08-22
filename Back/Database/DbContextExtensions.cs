using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Syki.Back.Features.Academic.CreateCourse;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Syki.Back.Features.Academic.CreateAcademicPeriod;

namespace Syki.Back.Database;

public static class DbContextExtensions
{
    extension(SykiDbContext ctx)
    {
        public Guid UserId => ctx.GetService<IHttpContextAccessor>().HttpContext.User.Id;
        public Guid InstitutionId => ctx.GetService<IHttpContextAccessor>().HttpContext.User.InstitutionId;
        public Guid CourseCurriculumId => ctx.GetService<IHttpContextAccessor>().HttpContext.User.CourseCurriculumId;
    }

    public static async Task<bool> CampusNotFound(this SykiDbContext ctx, Guid id)
    {
        var institutionId = ctx.InstitutionId;
        return !await ctx.Campi.AnyAsync(p => p.InstitutionId == institutionId && p.Id == id);
    }

    public static async Task<bool> CourseNotFound(this SykiDbContext ctx, Guid id)
    {
        var institutionId = ctx.InstitutionId;
        return !await ctx.Courses.AnyAsync(p => p.InstitutionId == institutionId && p.Id == id);
    }

    public static async Task<bool> CourseCurriculumNotFound(this SykiDbContext ctx, Guid id, Guid courseId)
    {
        var institutionId = ctx.InstitutionId;
        return !await ctx.CourseCurriculums.AnyAsync(g => g.InstitutionId == institutionId && g.Id == id && g.CourseId == courseId);
    }

    public static async Task<bool> AcademicPeriodNotFound(this SykiDbContext ctx, string id)
    {
        var institutionId = ctx.InstitutionId;
        return !await ctx.AcademicPeriods.AnyAsync(p => p.InstitutionId == institutionId && p.Id == id);
    }






    public static async Task<AcademicPeriod?> GetCurrentAcademicPeriod(this SykiDbContext ctx, Guid institutionId)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        return await ctx.AcademicPeriods
            .Where(p => p.InstitutionId == institutionId && p.StartAt <= today && p.EndAt >= today)
            .FirstOrDefaultAsync();
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
    )
    {
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
}
