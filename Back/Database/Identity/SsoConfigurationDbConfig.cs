using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class SsoConfigurationDbConfig : IEntityTypeConfiguration<SsoConfiguration>
{
    public void Configure(EntityTypeBuilder<SsoConfiguration> entity)
    {
        entity.ToTable("sso_configurations", DbSchemas.Estud);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.AllowedDomains)
            .WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(d => d.SsoConfigurationId);

        entity.HasIndex(e => e.PublicId)
            .IsUnique();
    }
}
