using Syki.Back.Domain.Classrooms;
using Syki.Back.Database.Classrooms;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<Classroom> Classrooms { get; set; }

    private static void ConfigureClassrooms(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClassroomDbConfig());
    }
}
