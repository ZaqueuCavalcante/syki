using Estud.Back.Domain.Campi;
using Estud.Back.Database.Campi;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Campus> Campi { get; set; }

    private static void ConfigureCampi(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CampusDbConfig());
    }
}
