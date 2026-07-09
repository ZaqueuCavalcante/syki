using Estud.Back.Domain.Students;
using Estud.Back.Database.Students;

namespace Estud.Back.Database;

public partial class EstudDbContext
{
    public DbSet<EstudStudent> Students { get; set; }
    public DbSet<StudentCourseEnrollment> StudentCourseEnrollments { get; set; }

    private static void ConfigureStudents(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EstudStudentDbConfig());
        modelBuilder.ApplyConfiguration(new StudentClassNoteDbConfig());
        modelBuilder.ApplyConfiguration(new StudentCourseEnrollmentDbConfig());
    }
}
