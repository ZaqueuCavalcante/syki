namespace Syki.Back.Commands;

public class CommandConfig : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> command)
    {
        command.ToTable("commands");

        command.HasKey(c => c.Id);
        command.Property(c => c.Id).ValueGeneratedNever();
    }
}
