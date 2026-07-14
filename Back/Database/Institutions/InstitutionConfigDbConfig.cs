using Estud.Back.Domain.Institutions;

namespace Estud.Back.Database.Institutions;

public class InstitutionConfigDbConfig : IEntityTypeConfiguration<InstitutionConfig>
{
    public void Configure(EntityTypeBuilder<InstitutionConfig> entity)
    {
        entity.ToTable("institution_configs", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.NoteLimit).HasPrecision(4, 2);
        entity.Property(e => e.FrequencyLimit).HasPrecision(5, 2);

        entity.HasIndex(e => e.InstitutionId).IsUnique();
    }
}
