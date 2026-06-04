using Npgsql;
using System.Data.Common;
using Syki.Back.Auth.Users;
using Audit.EntityFramework;
using Syki.Back.Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;

namespace Syki.Back.Database;

public partial class SykiDbContext(DbContextOptions<SykiDbContext> options, NpgsqlDataSource npgsqlDataSource, HybridCache cache)
    : IdentityDbContext<SykiUser, SykiRole, int, SykiUserClaim, SykiUserRole, SykiUserLogin, SykiRoleClaim, SykiUserToken>(options), IDataProtectionKeyContext
{
    public HybridCache Cache { get; set; } = cache;

    public RequestUser RequestUser { get; set; } = new();

    public string ActivityId { get; set; }
    public string Operation { get; set; }

    public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

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
        ConfigureClasses(modelBuilder);
        ConfigureCourses(modelBuilder);
        ConfigurePeriods(modelBuilder);
        ConfigureTeachers(modelBuilder);
        ConfigureStudents(modelBuilder);
        ConfigureCommands(modelBuilder);
        ConfigureIdentity(modelBuilder);
        ConfigureWebhooks(modelBuilder);
        ConfigureDisciplines(modelBuilder);
        ConfigureInstitutions(modelBuilder);
        ConfigureNotifications(modelBuilder);
        ConfigureCourseOfferings(modelBuilder);
        ConfigureCourseCurriculums(modelBuilder);
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
