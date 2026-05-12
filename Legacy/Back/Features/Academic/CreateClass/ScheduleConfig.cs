namespace Syki.Back.Features.Academic.CreateClass;

public class ScheduleConfig : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> schedule)
    {
        schedule.ToTable("schedules");

        schedule.HasKey(s => s.Id);
        schedule.Property(s => s.Id).ValueGeneratedNever();
    }
}
