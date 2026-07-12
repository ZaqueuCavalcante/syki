using Estud.Back.Domain.Calendar;

namespace Estud.Back.Database.Calendar;

public class CalendarDayDbConfig : IEntityTypeConfiguration<CalendarDay>
{
    public void Configure(EntityTypeBuilder<CalendarDay> entity)
    {
        entity.ToTable("calendar_days", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasIndex(e => new { e.InstitutionId, e.Date }).IsUnique();
    }
}
