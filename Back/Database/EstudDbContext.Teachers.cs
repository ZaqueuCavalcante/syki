using Estud.Back.Domain.Teachers;
using Estud.Back.Database.Teachers;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<EstudTeacher> Teachers { get; set; }
    public DbSet<TeacherCampus> TeachersCampi { get; set; }
    public DbSet<TeacherDiscipline> TeachersDisciplines { get; set; }

    private static void ConfigureTeachers(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EstudTeacherDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherCampusDbConfig());
        modelBuilder.ApplyConfiguration(new TeacherDisciplineDbConfig());
    }
}
