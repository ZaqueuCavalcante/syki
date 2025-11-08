using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class OrigemDbConfig : IEntityTypeConfiguration<Origem>
{
    public void Configure(EntityTypeBuilder<Origem> entity)
    {
        entity.ToTable("origem", "public", x =>
        {
            x.HasCheckConstraint("origem_nome_check", "char_length((nome)::text) <= 50");
        });

        entity.HasKey(e => e.OrigemId)
            .HasName("origem_pk");

        entity.Property(e => e.OrigemId)
            .ValueGeneratedNever()
            .HasColumnName("origem_id");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.HasIndex(e => e.Nome)
            .HasDatabaseName("origem_ak_nome")
            .IsUnique();
    }
}
