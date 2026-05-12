using Syki.Back.Domain.Identity;

namespace Syki.Back.Database.Identity;

public class SykiUserRoleDbConfig : IEntityTypeConfiguration<SykiUserRole>
{
    public void Configure(EntityTypeBuilder<SykiUserRole> entity)
    {
        entity.ToTable("user_roles", DbSchemas.Syki);

        entity.HasKey(e => new { e.InstitutionId, e.UserId, e.RoleId });

        entity.Property(e => e.InstitutionId).ValueGeneratedNever();
        entity.Property(e => e.UserId).ValueGeneratedNever();
        entity.Property(e => e.RoleId).ValueGeneratedNever();

        entity.HasOne(e => e.Institution)
            .WithMany()
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(e => e.InstitutionId);

        entity.HasOne(e => e.User)
            .WithMany()
            .HasPrincipalKey(c => c.Id)
            .HasForeignKey(e => e.UserId);

        entity.HasOne<SykiRole>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.RoleId);

        // Garante que cada usuário vai ter apenas uma role em cada org
        entity.HasIndex(e => new { e.InstitutionId, e.UserId })
            .IsUnique();
    }
}
