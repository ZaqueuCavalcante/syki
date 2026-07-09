using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SsoConfigurationDbConfig : IEntityTypeConfiguration<SsoConfiguration>
{
    public void Configure(EntityTypeBuilder<SsoConfiguration> entity)
    {
        entity.ToTable("sso_configurations", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasMany(e => e.AllowedDomains)
            .WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(d => d.SsoConfigurationId);

        entity.HasIndex(e => e.PublicId)
            .IsUnique();
    }
}
