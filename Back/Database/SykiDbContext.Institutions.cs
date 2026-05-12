using Syki.Back.Domain.Institutions;
using Syki.Back.Database.Institutions;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<Institution> Institutions { get; set; }

    private static void ConfigureInstitutions(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new InstitutionDbConfig());
    }
}
