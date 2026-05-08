using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Database.Commands;

public class CommandBatchDbConfig : IEntityTypeConfiguration<CommandBatch>
{
    public void Configure(EntityTypeBuilder<CommandBatch> commandBatch)
    {
        commandBatch.ToTable("command_batches");

        commandBatch.HasKey(c => c.Id);
        commandBatch.Property(c => c.Id).ValueGeneratedNever();
    }
}
