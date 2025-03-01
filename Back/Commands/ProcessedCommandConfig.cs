namespace Syki.Back.Commands;

public class ProcessedCommandConfig : IEntityTypeConfiguration<ProcessedCommand>
{
    public void Configure(EntityTypeBuilder<ProcessedCommand> command)
    {
        command.ToTable("processed_commands");

        command.HasKey(c => c.Id);
        command.Property(c => c.Id).ValueGeneratedNever();
    }
}
