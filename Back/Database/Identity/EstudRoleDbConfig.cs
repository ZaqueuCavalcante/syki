using Estud.Back.Domain.Identity;
using Estud.Back.Domain.Institutions;

namespace Estud.Back.Database.Identity;

public class EstudRoleDbConfig : IEntityTypeConfiguration<EstudRole>
{
    public void Configure(EntityTypeBuilder<EstudRole> entity)
    {
        entity.ToTable("roles", DbSchemas.Estud);

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
