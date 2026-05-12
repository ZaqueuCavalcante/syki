using Syki.Back.Commands.Domain.Commands;

namespace Syki.Back.Database.Commands;

public class CommandDbConfig : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> entity)
    {
        entity.ToTable("commands", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
