using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Identity;

public class SykiUserDbConfig : IEntityTypeConfiguration<SykiUser>
{
    public void Configure(EntityTypeBuilder<SykiUser> entity)
    {
        entity.ToTable("users", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.InstitutionId);
    }
}
