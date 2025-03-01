namespace Syki.Back.Commands;

public class CommandBatchConfig : IEntityTypeConfiguration<CommandBatch>
{
    public void Configure(EntityTypeBuilder<CommandBatch> commandBatch)
    {
        commandBatch.ToTable("command_batches");

        commandBatch.HasKey(c => c.Id);
        commandBatch.Property(c => c.Id).ValueGeneratedNever();
    }
}
