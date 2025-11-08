using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class PlanosRelatorioDbConfig : IEntityTypeConfiguration<PlanosRelatorio>
{
    public const string PlanosRelatorioIdSeq = "planos_faturamento_dossiers_id_seq";

    public void Configure(EntityTypeBuilder<PlanosRelatorio> entity)
    {
        entity.ToTable("planos_relatorios", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("planos_faturamento_dossiers_pkey");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .HasDefaultValueSql($"nextval('faturamento.{PlanosRelatorioIdSeq}')");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.Property(e => e.Padrao)
            .HasDefaultValue(false)
            .HasColumnName("padrao");
    }
}
