using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Database.Commands;

public class CommandBatchDbConfig : IEntityTypeConfiguration<CommandBatch>
{
    public void Configure(EntityTypeBuilder<CommandBatch> entity)
    {
        entity.ToTable("command_batches", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
