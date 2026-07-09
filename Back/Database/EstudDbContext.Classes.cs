using Estud.Back.Domain.Classes;
using Estud.Back.Database.Classes;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Class> Classes { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<ClassLesson> ClassLessons { get; set; }
    public DbSet<ClassLessonAttendance> ClassLessonAttendances { get; set; }

    private static void ConfigureClasses(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClassDbConfig());
        modelBuilder.ApplyConfiguration(new ScheduleDbConfig());
        modelBuilder.ApplyConfiguration(new ClassLessonDbConfig());
        modelBuilder.ApplyConfiguration(new ClassStudentDbConfig());
        modelBuilder.ApplyConfiguration(new ClassActivityDbConfig());
        modelBuilder.ApplyConfiguration(new ClassActivityWorkDbConfig());
        modelBuilder.ApplyConfiguration(new ClassLessonAttendanceDbConfig());
    }
}
