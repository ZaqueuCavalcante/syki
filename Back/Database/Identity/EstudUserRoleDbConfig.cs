using Estud.Back.Domain.Identity;

namespace Estud.Back.Database.Identity;

public class EstudUserRoleDbConfig : IEntityTypeConfiguration<EstudUserRole>
{
    public void Configure(EntityTypeBuilder<EstudUserRole> entity)
    {
        entity.ToTable("user_roles", DbSchemas.Estud);

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

        entity.HasOne(e => e.Role)
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.RoleId);

        // Garante que cada usuário vai ter apenas uma role em cada instituição
        entity.HasIndex(e => new { e.InstitutionId, e.UserId })
            .IsUnique();
    }
}
