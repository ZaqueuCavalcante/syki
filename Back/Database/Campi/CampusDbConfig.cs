using Estud.Back.Domain.Campi;

namespace Estud.Back.Database.Campi;

public class CampusDbConfig : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> entity)
    {
        entity.ToTable("campi", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
    }
}
