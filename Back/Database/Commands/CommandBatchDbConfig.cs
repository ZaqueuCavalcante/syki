using Estud.Back.Domain.Commands;

namespace Estud.Back.Database.Commands;

public class CommandBatchDbConfig : IEntityTypeConfiguration<CommandBatch>
{
    public void Configure(EntityTypeBuilder<CommandBatch> entity)
    {
        entity.ToTable("command_batches", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
