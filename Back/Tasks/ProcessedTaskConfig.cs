namespace Syki.Back.Tasks;

public class ProcessedTaskConfig : IEntityTypeConfiguration<ProcessedTask>
{
    public void Configure(EntityTypeBuilder<ProcessedTask> task)
    {
        task.ToTable("processed_tasks");

        task.HasKey(t => t.Id);
        task.Property(t => t.Id).ValueGeneratedNever();
    }
}
