using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ConsultaDetalheTipoDbConfig : IEntityTypeConfiguration<ConsultaDetalheTipo>
{
    public void Configure(EntityTypeBuilder<ConsultaDetalheTipo> entity)
    {
        entity.ToTable("consulta_detalhe_tipo", "public");

        entity.HasKey(e => e.ConsultaDetalheTipoId)
            .HasName("consulta_detalhe_tipo_pk");

        entity.Property(e => e.ConsultaDetalheTipoId)
            .ValueGeneratedNever()
            .HasColumnName("consulta_detalhe_tipo_id");

        entity.Property(e => e.Nome)
            .HasMaxLength(50)
            .HasColumnName("nome");
        
        entity.HasIndex(e => e.Nome, "consulta_detalhe_tipo_ak_nome")
            .IsUnique();
    }
}
