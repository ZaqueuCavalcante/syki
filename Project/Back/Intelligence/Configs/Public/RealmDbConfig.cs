using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class RealmDbConfig : IEntityTypeConfiguration<Realm>
{
    public void Configure(EntityTypeBuilder<Realm> entity)
    {
        entity.ToTable("realms", "public");

        entity.HasKey(e => e.Id)
            .HasName("realms_pkey");
 
        entity.Property(e => e.Id)
            .ValueGeneratedNever()
            .HasColumnName("id");

        entity.Property(e => e.CreatedAt)
            .HasColumnType("timestamp(6)")
            .HasColumnName("created_at");

        entity.Property(e => e.Description)
            .HasColumnType("citext")
            .HasColumnName("description");

        entity.Property(e => e.Name)
            .HasColumnType("citext")
            .HasColumnName("name");

        entity.Property(e => e.Uid)
            .HasDefaultValueSql("uuid_generate_v4()")
            .HasColumnName("uid");
    }
}
