using Estud.Back.Domain.Periods;

namespace Estud.Back.Database.Periods;

public class EnrollmentPeriodDbConfig : IEntityTypeConfiguration<EnrollmentPeriod>
{
    public void Configure(EntityTypeBuilder<EnrollmentPeriod> entity)
    {
        entity.ToTable("enrollment_periods", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
