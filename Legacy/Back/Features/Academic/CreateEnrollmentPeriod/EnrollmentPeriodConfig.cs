namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

public class EnrollmentPeriodConfig : IEntityTypeConfiguration<EnrollmentPeriod>
{
    public void Configure(EntityTypeBuilder<EnrollmentPeriod> period)
    {
        period.ToTable("enrollment_periods");

        period.HasKey(p => new { p.Id, p.InstitutionId });
    }
}
