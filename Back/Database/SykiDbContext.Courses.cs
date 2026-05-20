using Syki.Back.Domain.Courses;
using Syki.Back.Database.Courses;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<CourseDiscipline> CoursesDisciplines { get; set; }

    private static void ConfigureCourses(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseDbConfig());
        modelBuilder.ApplyConfiguration(new CourseOfferingDbConfig());
        modelBuilder.ApplyConfiguration(new CourseCurriculumDbConfig());
        modelBuilder.ApplyConfiguration(new CourseDisciplineDbConfig());
        modelBuilder.ApplyConfiguration(new CourseCurriculumDisciplineDbConfig());
    }
}
