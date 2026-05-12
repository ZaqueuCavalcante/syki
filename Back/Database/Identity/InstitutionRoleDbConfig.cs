using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Identity;

public class InstitutionRoleDbConfig : IEntityTypeConfiguration<InstitutionRole>
{
    public void Configure(EntityTypeBuilder<InstitutionRole> entity)
    {
        entity.ToTable("institution_roles", DbSchemas.Syki);

        entity.HasKey(e => new { e.InstitutionId, e.RoleId });

        entity.Property(e => e.InstitutionId)
            .ValueGeneratedNever();

        entity.Property(e => e.RoleId)
            .ValueGeneratedNever();

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.InstitutionId);

        entity.HasOne(e => e.Role)
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.RoleId);
    }
}
