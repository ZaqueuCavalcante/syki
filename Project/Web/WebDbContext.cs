using Exato.Web.Domain;
using Exato.Web.Configs;

namespace Exato.Web;

public class WebDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<UserEmail> UserEmails { get; set; }
    public DbSet<UserPhoneNumber> UserPhoneNumbers { get; set; }
    public DbSet<WebUserCompany> WebUserCompanies { get; set; }
    public DbSet<WebUser> Users { get; set; }
    public DbSet<ActivitiesLog> ActivitiesLogs { get; set; }

    public WebDbContext(DbContextOptions<WebDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        optionsBuilder.UseNpgsql(x => x.MigrationsHistoryTable("migrations"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("public");

        modelBuilder.ApplyConfiguration(new CompanyDbConfig());

        modelBuilder.ApplyConfiguration(new WebUserCompanyDbConfig());

        modelBuilder.ApplyConfiguration(new WebUserDbConfig());
        modelBuilder.ApplyConfiguration(new UserEmailDbConfig());
        modelBuilder.ApplyConfiguration(new UserPhoneNumberDbConfig());

        modelBuilder.ApplyConfiguration(new ActivitiesLogDbConfig());

        modelBuilder.ApplyConfiguration(new PaymentDbConfig());

        FixNames(modelBuilder);
        AddPostgresExtensions(modelBuilder);
    }

    private static void AddPostgresExtensions(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("citext")
            .HasPostgresExtension("uuid-ossp");
    }

    private static void FixNames(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName().StringIsEmpty()) continue;

            foreach (var fk in entity.GetForeignKeys())
            {
                if (fk.GetConstraintName().StringHasValue())
                {
                    fk.SetConstraintName(fk.GetConstraintName()!.Replace("~", ""));
                }
            }
        }
    }

    public async Task<int> SaveChangesAsync<TEntity>(TEntity entity)
    {
        Add(entity);
        return await SaveChangesAsync();
    }
}
