using Syki.Back.Database.Classes;
using Syki.Back.Commands.Domain.Classes;

namespace Syki.Back.Database;

public partial class SykiDbContext
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
        modelBuilder.ApplyConfiguration(new ClassLessonAttendanceDbConfig());
    }
}
