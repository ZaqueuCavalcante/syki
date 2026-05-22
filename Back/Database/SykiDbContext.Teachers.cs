using Syki.Back.Domain.Teachers;
using Syki.Back.Database.Teachers;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<SykiTeacher> Teachers { get; set; }
    public DbSet<TeacherDiscipline> TeachersDisciplines { get; set; }

    private static void ConfigureTeachers(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SykiTeacherDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherCampusDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherDisciplineDbConfig());
    }
}
