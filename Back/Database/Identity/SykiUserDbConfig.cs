using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SykiUserDbConfig : IEntityTypeConfiguration<SykiUser>
{
    public void Configure(EntityTypeBuilder<SykiUser> entity)
    {
        entity.ToTable("users", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne(e => e.Institution)
            .WithMany()
            .HasPrincipalKey(i => i.Id)
            .HasForeignKey(e => e.InstitutionId);
    }
}
