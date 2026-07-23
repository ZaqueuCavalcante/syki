using Npgsql;
using System.Data.Common;
using Estud.Back.Auth.Users;
using Audit.EntityFramework;
using Estud.Back.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Estud.Back.Database;

public partial class EstudDbContext(DbContextOptions<EstudDbContext> options, NpgsqlDataSource npgsqlDataSource, HybridCache cache)
    : IdentityDbContext<EstudUser, EstudRole, int, EstudUserClaim, EstudUserRole, EstudUserLogin, EstudRoleClaim, EstudUserToken>(options), IDataProtectionKeyContext
{
    public HybridCache Cache { get; set; } = cache;

    public RequestUser RequestUser { get; set; } = new();

    public string ActivityId { get; set; }
    public string Operation { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(npgsqlDataSource, x => x.MigrationsHistoryTable("migrations", DbSchemas.Estud));

        optionsBuilder.AddInterceptors(new SaveDomainEventsInterceptor());
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureAudit(modelBuilder);
        ConfigureCampi(modelBuilder);
        ConfigureClasses(modelBuilder);
        ConfigureCourses(modelBuilder);
        ConfigurePeriods(modelBuilder);
        ConfigureParents(modelBuilder);
        ConfigureTeachers(modelBuilder);
        ConfigureStudents(modelBuilder);
        ConfigureCommands(modelBuilder);
        ConfigureCalendar(modelBuilder);
        ConfigureIdentity(modelBuilder);
        ConfigureWebhooks(modelBuilder);
        ConfigureActivities(modelBuilder);
        ConfigureClassrooms(modelBuilder);
        ConfigureDisciplines(modelBuilder);
        ConfigureInstitutions(modelBuilder);
        ConfigureDomainEvents(modelBuilder);
        ConfigureNotifications(modelBuilder);
        ConfigureCourseOfferings(modelBuilder);
        ConfigureCourseCurriculums(modelBuilder);

        ConfigureDatabaseConventions(modelBuilder);
    }

    private static void ConfigureDatabaseConventions(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName().IsEmpty()) continue;

            if (typeof(DomainEntity).IsAssignableFrom(entity.ClrType))
            {
                var uid = entity.FindProperty(nameof(DomainEntity.Uid))!;
                uid.SetMaxLength(26);
                entity.AddIndex(uid).IsUnique = true;
            }

            entity.SetTableName(entity.GetTableName().ToSnakeCase().Replace("asp_net_", ""));

            foreach (var fk in entity.GetForeignKeys())
            {
                if (fk.GetConstraintName().HasValue())
                {
                    fk.SetConstraintName(fk.GetConstraintName()!.Replace("~", "").Replace("1", ""));
                }
            }

            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(index.GetDatabaseName()?.ToSnakeCase());
            }
        }
    }

    public async Task<DbConnection> GetOpenConnectionAsync(CancellationToken ct = default)
    {
        await Database.OpenConnectionAsync(ct);
        return Database.GetDbConnection();
    }

    public async Task<int> SaveChangesAsync<TEntity>(TEntity entity)
    {
        Add(entity);
        return await SaveChangesAsync();
    }
}
