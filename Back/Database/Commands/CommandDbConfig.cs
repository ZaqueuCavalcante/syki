using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Database.Commands;

public class CommandDbConfig : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> command)
    {
        command.ToTable("commands");

        command.HasKey(c => c.Id);
        command.Property(c => c.Id).ValueGeneratedNever();
    }
}
