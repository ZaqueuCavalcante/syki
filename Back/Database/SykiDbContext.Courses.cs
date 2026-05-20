using Syki.Back.Database.Courses;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    private static void ConfigureCourses(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CourseDbConfig());
        modelBuilder.ApplyConfiguration(new CourseOfferingDbConfig());
        modelBuilder.ApplyConfiguration(new CourseCurriculumDbConfig());
        modelBuilder.ApplyConfiguration(new CourseDisciplineDbConfig());
    }
}
