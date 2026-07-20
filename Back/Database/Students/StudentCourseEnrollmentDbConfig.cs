using Estud.Back.Domain.Students;

namespace Estud.Back.Database.Students;

public class StudentCourseEnrollmentDbConfig : IEntityTypeConfiguration<StudentCourseEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentCourseEnrollment> entity)
    {
        entity.ToTable("student_course_enrollments", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasOne<EstudStudent>()
            .WithMany()
            .HasForeignKey(e => e.StudentId);

        entity.HasOne(e => e.CourseOffering)
            .WithMany()
            .HasForeignKey(e => e.CourseOfferingId);
    }
}
