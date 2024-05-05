namespace Syki.Back.Tasks;

public class SykiTaskConfig : IEntityTypeConfiguration<SykiTask>
{
    public void Configure(EntityTypeBuilder<SykiTask> task)
    {
        task.ToTable("tasks");

        task.HasKey(t => t.Id);
        task.Property(t => t.Id).ValueGeneratedNever();
    }
}
