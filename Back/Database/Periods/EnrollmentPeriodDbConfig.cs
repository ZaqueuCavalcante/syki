using Syki.Back.Domain.Periods;

namespace Syki.Back.Database.Periods;

public class EnrollmentPeriodDbConfig : IEntityTypeConfiguration<EnrollmentPeriod>
{
    public void Configure(EntityTypeBuilder<EnrollmentPeriod> entity)
    {
        entity.ToTable("enrollment_periods", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
