using Exato.Back.Intelligence.Domain.Public;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Public;

public class ConsultaResultadoTipoDbConfig : IEntityTypeConfiguration<ConsultaResultadoTipo>
{
    public void Configure(EntityTypeBuilder<ConsultaResultadoTipo> entity)
    {
        entity.ToTable("consulta_resultado_tipo", "public", x =>
        {
            x.HasCheckConstraint("consulta_resultado_tipo_nome_check", "char_length((nome)::text) <= 80");
        });

        entity.HasKey(e => e.ConsultaResultadoTipoId)
            .HasName("consulta_resultado_tipo_pk");

        entity.Property(e => e.ConsultaResultadoTipoId)
            .ValueGeneratedNever()
            .HasColumnName("consulta_resultado_tipo_id");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.Property(e => e.Definitivo)
            .HasColumnName("definitivo");

        entity.Property(e => e.Erro)
            .HasColumnName("erro");

        entity.Property(e => e.Faturavel)
            .HasColumnName("faturavel");

        entity.Property(e => e.GeraComprovante)
            .HasColumnName("gera_comprovante");

        entity.Property(e => e.GeraRegistroConsulta)
            .HasColumnName("gera_registro_consulta");

        entity.HasIndex(e => e.Nome, "consulta_resultado_tipo_ak_nome")
            .IsUnique();
    }
}
