using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class FaturamentoTipoDbConfig : IEntityTypeConfiguration<FaturamentoTipo>
{
    public void Configure(EntityTypeBuilder<FaturamentoTipo> entity)
    {
        entity.ToTable("faturamento_tipo", "public", x =>
        {
            x.HasCheckConstraint("faturamento_tipo_nome_check", "char_length((nome)::text) <= 50");
        });

        entity.HasKey(e => e.FaturamentoTipoId)
            .HasName("faturamento_tipo_pk");

        entity.Property(e => e.FaturamentoTipoId)
            .ValueGeneratedNever()
            .HasColumnName("faturamento_tipo_id");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.HasIndex(e => e.Nome, "faturamento_tipo_ak_nome")
            .IsUnique();
    }
}
