using Estud.Back.Domain.Commands;

namespace Estud.Back.Database.Commands;

public class CommandDbConfig : IEntityTypeConfiguration<Command>
{
    public void Configure(EntityTypeBuilder<Command> entity)
    {
        entity.ToTable("commands", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
