using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class ValorTotalDbConfig : IEntityTypeConfiguration<ValorTotal>
{
    public const string ValorTotalIdSeq = "valor_total_faturamento_id_seq";

    public void Configure(EntityTypeBuilder<ValorTotal> entity)
    {
        entity.ToTable("valor_total", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("valor_total_faturamento_pkey");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .HasDefaultValueSql($"nextval('faturamento.{ValorTotalIdSeq}')");

        entity.Property(e => e.AlteradoEm)
            .HasColumnType("timestamp")
            .HasColumnName("alterado_em");

        entity.Property(e => e.AnoMes)
            .HasColumnName("ano_mes");

        entity.Property(e => e.ContractCode)
            .HasColumnName("contract_code");

        entity.Property(e => e.FranquiaMinima)
            .HasColumnName("franquia_minima");

        entity.Property(e => e.InseridoEm)
            .HasColumnType("timestamp")
            .HasColumnName("inserido_em");

        entity.Property(e => e.NfGroup)
            .HasColumnName("nf_group");

        entity.Property(e => e.ParentOrganizationId)
            .HasColumnName("parent_organization_id");

        entity.Property(e => e.ValorConsumo)
            .HasColumnName("valor_consumo");

        entity.Property(e => e.ValorFinal)
            .HasColumnName("valor_final");

        entity.Property(e => e.ValorTotalCreditos)
            .HasColumnName("valor_total_creditos");

        entity.Property(e => e.ValorTotalDoccheck)
            .HasColumnName("valor_total_doccheck");

        entity.Property(e => e.ValorTotalRelatorios)
            .HasColumnName("valor_total_relatorios");

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(o => o.ClienteId)
            .HasForeignKey(e => e.ParentOrganizationId);

        entity.HasIndex(e => new { e.ParentOrganizationId, e.AnoMes, e.ContractCode })
            .HasDatabaseName("unique_parent_anomes_contrato")
            .IsUnique();
    }
}
