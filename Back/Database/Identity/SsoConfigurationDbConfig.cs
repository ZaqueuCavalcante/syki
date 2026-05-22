using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Identity;

public class SsoConfigurationDbConfig : IEntityTypeConfiguration<SsoConfiguration>
{
    public void Configure(EntityTypeBuilder<SsoConfiguration> entity)
    {
        entity.ToTable("sso_configurations", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(i => i.Id)
            .HasForeignKey(e => e.InstitutionId);

        entity.HasMany(e => e.AllowedDomains)
            .WithOne()
            .HasPrincipalKey(e => e.Id)
            .HasForeignKey(d => d.SsoConfigurationId);

        entity.HasIndex(e => e.PublicId)
            .IsUnique();
    }
}
