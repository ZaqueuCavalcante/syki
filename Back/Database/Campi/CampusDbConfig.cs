using Syki.Back.Domain.Campi;

namespace Syki.Back.Database.Campi;

public class CampusDbConfig : IEntityTypeConfiguration<Campus>
{
    public void Configure(EntityTypeBuilder<Campus> entity)
    {
        entity.ToTable("campi");

        entity.HasKey(e => e.Id);
    }
}
