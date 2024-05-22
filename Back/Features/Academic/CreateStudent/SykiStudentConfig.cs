using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateStudent;

public class SykiStudentConfig : IEntityTypeConfiguration<SykiStudent>
{
    public void Configure(EntityTypeBuilder<SykiStudent> student)
    {
        student.ToTable("students");

        student.HasKey(a => a.Id);
        student.Property(a => a.Id).ValueGeneratedNever();

        student.Property(a => a.CourseOfferingId);
        student.Property(a => a.Name);
        student.Property(a => a.EnrollmentCode);

        student.HasOne(a => a.User)
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<SykiStudent>(a => new { a.InstitutionId, a.Id });
    }
}
