using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class SsoAllowedDomainDbConfig : IEntityTypeConfiguration<SsoAllowedDomain>
{
    public void Configure(EntityTypeBuilder<SsoAllowedDomain> entity)
    {
        entity.ToTable("sso_allowed_domains", DbSchemas.Estud);

        entity.HasKey(e => e.Domain);
        entity.Property(e => e.Domain).ValueGeneratedNever();

        entity.HasOne<SsoConfiguration>()
            .WithMany(c => c.AllowedDomains)
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(e => e.SsoConfigurationId);
    }
}
