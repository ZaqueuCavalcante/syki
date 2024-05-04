using Syki.Back.CreateEnrollmentPeriod;

namespace Syki.Back.CreateAcademicPeriod;

public class AcademicPeriodConfig : IEntityTypeConfiguration<AcademicPeriod>
{
    public void Configure(EntityTypeBuilder<AcademicPeriod> period)
    {
        period.ToTable("academic_periods");

        period.HasKey(p => new { p.Id, p.InstitutionId });

        period.HasOne<EnrollmentPeriod>()
            .WithOne()
            .HasPrincipalKey<AcademicPeriod>(ap => new { ap.Id, ap.InstitutionId })
            .HasForeignKey<EnrollmentPeriod>(ep => new { ep.Id, ep.InstitutionId });
    }
}
