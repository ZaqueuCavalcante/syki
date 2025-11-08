using Npgsql;
using Audit.EntityFramework;
using Exato.Back.Database.Interceptors;
using Exato.Back.Features.Cross.CreateExatoUser;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Exato.Back.Database;

public partial class BackDbContext(DbContextOptions<BackDbContext> options, NpgsqlDataSource npgsqlDataSource)
    : IdentityDbContext<ExatoUser, ExatoRole, Guid>(options)
{
    public Guid UserId { get; set; }
    public int OrganizationId { get; set; }
    public string ActivityId { get; set; }
    public string Operation { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();

        optionsBuilder.UseNpgsql(
            npgsqlDataSource,
            x => x.MigrationsHistoryTable("migrations", "exato")
        );

        optionsBuilder.AddInterceptors(new SetBatchSizeInterceptor());
        optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor());
        optionsBuilder.AddInterceptors(new SaveDomainEventsInterceptor());
        optionsBuilder.AddInterceptors(new CommandsDelayInterceptor());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigurePublicSchema(modelBuilder);
        ConfigureIbgeSchema(modelBuilder);
        ConfigureFaturamentoSchema(modelBuilder);

        ConfigureExatoSchema(modelBuilder);

        FixNames(modelBuilder);
        AddPostgresExtensions(modelBuilder);
    }

    private static void FixNames(ModelBuilder modelBuilder)
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

    public async Task<int> SaveChangesAsync<TEntity>(TEntity entity)
    {
        Add(entity);
        return await SaveChangesAsync();
    }
}
