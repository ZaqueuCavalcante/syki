using Syki.Back.Database.Teachers;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigureTeachers(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SykiTeacherDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherCampusDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherDisciplineDbConfig());
    }
}
