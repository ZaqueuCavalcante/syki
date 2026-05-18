using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class MagicLinkDbConfig : IEntityTypeConfiguration<MagicLink>
{
    public void Configure(EntityTypeBuilder<MagicLink> entity)
    {
        entity.ToTable("magic_links", DbSchemas.Syki);

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.HasOne(e => e.User)
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);
    }
}
