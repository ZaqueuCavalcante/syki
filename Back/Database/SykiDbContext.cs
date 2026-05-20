using Npgsql;
using System.Data.Common;
using Syki.Back.Auth.Users;
using Audit.EntityFramework;
using Syki.Back.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Syki.Back.Database;

public partial class SykiDbContext(DbContextOptions<SykiDbContext> options, NpgsqlDataSource npgsqlDataSource, HybridCache cache)
    : IdentityDbContext<SykiUser, SykiRole, int, SykiUserClaim, SykiUserRole, SykiUserLogin, SykiRoleClaim, SykiUserToken>(options)
{
    public HybridCache Cache { get; set; } = cache;

    public RequestUser RequestUser { get; set; } = new();

    public string ActivityId { get; set; }
    public string Operation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(npgsqlDataSource, x => x.MigrationsHistoryTable("migrations", DbSchemas.Syki));

        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureAudit(modelBuilder);
        ConfigureCampi(modelBuilder);
        ConfigureCourses(modelBuilder);
        ConfigurePeriods(modelBuilder);
        ConfigureTeachers(modelBuilder);
        ConfigureStudents(modelBuilder);
        ConfigureCommands(modelBuilder);
        ConfigureIdentity(modelBuilder);
        ConfigureDisciplines(modelBuilder);
        ConfigureInstitutions(modelBuilder);

        ConfigureDatabaseNames(modelBuilder);
    }

    private static void ConfigureDatabaseNames(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName().IsEmpty()) continue;

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
