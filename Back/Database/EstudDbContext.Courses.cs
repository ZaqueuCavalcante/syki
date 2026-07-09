using Estud.Back.Domain.Courses;
using Estud.Back.Database.Courses;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseDiscipline> CoursesDisciplines { get; set; }

    private static void ConfigureCourses(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseDbConfig());
        modelBuilder.ApplyConfiguration(new CourseDisciplineDbConfig());
    }
}
