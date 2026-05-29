using Syki.Back.Domain.Identity;
using Syki.Back.Domain.Institutions;

namespace Syki.Back.Database.Identity;

public class SykiRoleDbConfig : IEntityTypeConfiguration<SykiRole>
{
    public void Configure(EntityTypeBuilder<SykiRole> entity)
    {
        entity.ToTable("roles", DbSchemas.Syki);

        entity.HasKey(e => e.Id);

        entity.Property(e => e.Permissions)
            .HasColumnType("integer[]")
            .IsRequired();

        entity.HasOne<Institution>()
            .WithMany()
            .HasPrincipalKey(u => u.Id)
            .HasForeignKey(e => e.OwnerId);

        // Remove o índice único padrão do ASP.NET Identity
        entity.HasIndex(e => e.NormalizedName)
            .IsUnique(false)
            .HasDatabaseName("role_name_index");

        // Índice único composto: permite mesmo nome em orgs diferentes
        // AreNullsDistinct(false) gera NULLS NOT DISTINCT no PostgreSQL 15+
        entity.HasIndex(e => new { e.OwnerId, e.NormalizedName })
            .IsUnique()
            .AreNullsDistinct(false);
    }
}
