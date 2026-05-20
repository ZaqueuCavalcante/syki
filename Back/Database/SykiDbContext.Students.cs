using Syki.Back.Database.Students;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigureStudents(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SykiStudentDbConfig());
    }
}
