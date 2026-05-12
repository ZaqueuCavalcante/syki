using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Institutions;

public class InstitutionDbConfig : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> entity)
    {
        entity.ToTable("institutions", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
    }
}
