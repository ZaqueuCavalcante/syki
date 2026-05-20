using Syki.Back.Database.Disciplines;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigureDisciplines(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DisciplineDbConfig());
    }
}
