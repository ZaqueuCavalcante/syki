using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class PrecificacaoDbConfig : IEntityTypeConfiguration<Precificacao>
{
    public void Configure(EntityTypeBuilder<Precificacao> entity)
    {
        entity.ToTable("precificacao", "faturamento", x =>
        {
            x.HasCheckConstraint("precificacao_check", "(fim_faixa IS NULL) OR (fim_faixa > inicio_faixa)");
        });

        entity.HasKey(e => e.Id)
            .HasName("precificacao_pkey");

        entity.Property(e => e.Id)
            .UseSerialColumn()
            .HasColumnName("id");

        entity.Property(e => e.ConsultaTipoId)
            .HasColumnName("consulta_tipo_id");

        entity.Property(e => e.FaixasId)
            .HasColumnName("faixas_id");

        entity.Property(e => e.FimFaixa)
            .HasColumnName("fim_faixa");

        entity.Property(e => e.InicioFaixa)
            .HasColumnName("inicio_faixa");

        entity.Property(e => e.PlanoId)
            .HasColumnName("plano_id");

        entity.Property(e => e.ValorUnitario)
            .HasPrecision(10, 3)
            .HasColumnName("valor_unitario");

        entity.HasOne<Plano>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(x => x.PlanoId);

        // TODO: Fix this
        // ConsultaTipo.ConsultaTipoId é short (smallint no postgres)
        // Precificacao.ConsultaTipoId é int (integer no postgres)
        // entity.HasOne<ConsultaTipo>()
        //     .WithMany()
        //     .HasPrincipalKey(x => x.ConsultaTipoId)
        //     .HasForeignKey(x => x.ConsultaTipoId);

        entity.HasIndex(e => new { e.PlanoId, e.ConsultaTipoId, e.FaixasId, e.ValorUnitario }, "uq_pct_plano_consulta_faixa_preco")
            .IsUnique();
    }
}
