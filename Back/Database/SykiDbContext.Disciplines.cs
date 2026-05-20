using Syki.Back.Domain.Disciplines;
using Syki.Back.Database.Disciplines;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<Discipline> Disciplines { get; set; }

    private static void ConfigureDisciplines(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DisciplineDbConfig());
    }
}
