using Syki.Back.Domain.Students;
using Syki.Back.Database.Students;

namespace Syki.Back.Database;

public partial class SykiDbContext
{
    public DbSet<StudentCourseEnrollment> StudentCourseEnrollments { get; set; }

    private static void ConfigureStudents(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SykiStudentDbConfig());
        modelBuilder.ApplyConfiguration(new StudentCourseEnrollmentDbConfig());
    }
}
