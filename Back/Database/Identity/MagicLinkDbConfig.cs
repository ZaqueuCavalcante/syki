using Syki.Back.Domain.Identity;
using Syki.Back.Features.Cross.CreateUser;

namespace Syki.Back.Database.Identity;

public class MagicLinkDbConfig : IEntityTypeConfiguration<MagicLink>
{
    public void Configure(EntityTypeBuilder<MagicLink> entity)
    {
        entity.ToTable("magic_links");

        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).ValueGeneratedNever();

        entity.HasOne<SykiUser>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.UserId);
    }
}
