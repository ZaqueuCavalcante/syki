using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Features.Academic.CreateStudent;

public class SykiStudentConfig : IEntityTypeConfiguration<SykiStudent>
{
    public void Configure(EntityTypeBuilder<SykiStudent> student)
    {
        student.ToTable("students");

        student.HasKey(s => s.Id);
        student.Property(s => s.Id).ValueGeneratedNever();

        student.HasOne(s => s.User)
            .WithOne()
            .HasPrincipalKey<SykiUser>(u => new { u.InstitutionId, u.Id })
            .HasForeignKey<SykiStudent>(s => new { s.InstitutionId, s.Id });

        student.HasOne(s => s.CourseOffering)
            .WithMany()
            .HasForeignKey(s => s.CourseOfferingId);

        student.Property(x => x.YieldCoefficient).HasPrecision(4, 2);

        student.HasIndex(s => s.EnrollmentCode).IsUnique();
    }
}
