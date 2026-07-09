using Estud.Back.Domain.Disciplines;
using Estud.Back.Database.Disciplines;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Discipline> Disciplines { get; set; }

    private static void ConfigureDisciplines(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new DisciplineDbConfig());
    }
}
