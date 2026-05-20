using Syki.Back.Domain.Periods;

namespace Syki.Back.Database.Periods;

public class AcademicPeriodDbConfig : IEntityTypeConfiguration<AcademicPeriod>
{
    public void Configure(EntityTypeBuilder<AcademicPeriod> entity)
    {
        entity.ToTable("academic_periods", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
