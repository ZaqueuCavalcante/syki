using Estud.Back.Domain.Classes;

namespace Estud.Back.Database.Classes;

public class ScheduleDbConfig : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> entity)
    {
        entity.ToTable("schedules", DbSchemas.Estud);

        entity.HasKey(s => s.Id);
    }
}
