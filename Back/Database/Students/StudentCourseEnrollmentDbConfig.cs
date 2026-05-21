using Syki.Back.Domain.Courses;
using Syki.Back.Domain.Students;

namespace Syki.Back.Database.Students;

public class StudentCourseEnrollmentDbConfig : IEntityTypeConfiguration<StudentCourseEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentCourseEnrollment> entity)
    {
        entity.ToTable("student_course_enrollments", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne<SykiStudent>()
            .WithMany()
            .HasForeignKey(e => e.StudentId);

        entity.HasOne<CourseOffering>()
            .WithMany()
            .HasForeignKey(e => e.CourseOfferingId);
    }
}
