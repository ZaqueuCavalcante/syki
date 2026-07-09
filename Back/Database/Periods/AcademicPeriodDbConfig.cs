using Estud.Back.Domain.Periods;

namespace Estud.Back.Database.Periods;

public class AcademicPeriodDbConfig : IEntityTypeConfiguration<AcademicPeriod>
{
    public void Configure(EntityTypeBuilder<AcademicPeriod> entity)
    {
        entity.ToTable("academic_periods", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
