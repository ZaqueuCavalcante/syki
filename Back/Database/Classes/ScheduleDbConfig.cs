using Estud.Back.Domain.Classes;
using Estud.Back.Domain.Teachers;

namespace Estud.Back.Database.Classes;

public class ScheduleDbConfig : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> entity)
    {
        entity.ToTable("schedules", DbSchemas.Estud);

        entity.HasKey(s => s.Id);

        entity.HasOne<EstudTeacher>()
            .WithMany()
            .HasForeignKey(s => s.TeacherId);
    }
}
