using Estud.Back.Domain.Classrooms;
using Estud.Back.Database.Classrooms;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Classroom> Classrooms { get; set; }

    private static void ConfigureClassrooms(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClassroomDbConfig());
        modelBuilder.ApplyConfiguration(new ClassroomClassDbConfig());
    }
}
