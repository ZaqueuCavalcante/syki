using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class MagicLinkDbConfig : IEntityTypeConfiguration<MagicLink>
{
    public void Configure(EntityTypeBuilder<MagicLink> entity)
    {
        entity.ToTable("magic_links", DbSchemas.Estud);

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.HasOne(e => e.User)
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);
    }
}
