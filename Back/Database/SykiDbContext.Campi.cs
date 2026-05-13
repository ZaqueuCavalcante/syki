using Syki.Back.Domain.Campi;
using Syki.Back.Database.Campi;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<Campus> Campi { get; set; }

    private static void ConfigureCampi(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CampusDbConfig());
    }
}
