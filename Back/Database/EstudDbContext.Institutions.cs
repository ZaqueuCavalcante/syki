using Estud.Back.Domain.Institutions;
using Estud.Back.Database.Institutions;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Institution> Institutions { get; set; }
    public DbSet<InstitutionConfig> InstitutionConfigs { get; set; }

    private static void ConfigureInstitutions(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InstitutionDbConfig());
        modelBuilder.ApplyConfiguration(new InstitutionConfigDbConfig());
    }
}
