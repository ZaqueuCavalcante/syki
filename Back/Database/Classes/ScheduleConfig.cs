using Syki.Back.Commands.Domain.Classes;

namespace Syki.Back.Database.Classes;

public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> entity)
    {
        entity.ToTable("schedules", DbSchemas.Syki);

        entity.HasKey(s => s.Id);
    }
}
