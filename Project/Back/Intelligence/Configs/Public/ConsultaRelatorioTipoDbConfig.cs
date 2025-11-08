using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ConsultaRelatorioTipoDbConfig : IEntityTypeConfiguration<ConsultaRelatorioTipo>
{
    public void Configure(EntityTypeBuilder<ConsultaRelatorioTipo> entity)
    {
        entity.ToTable("consulta_relatorio_tipo", "public");

        entity.HasKey(e => e.Id)
            .HasName("consulta_relatorio_tipo_pkey");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.Tipo)
            .HasColumnName("tipo");
    }
}
