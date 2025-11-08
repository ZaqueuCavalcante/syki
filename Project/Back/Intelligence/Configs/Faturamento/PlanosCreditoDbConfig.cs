using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class PlanosCreditoDbConfig : IEntityTypeConfiguration<PlanosCredito>
{
    public const string PlanosCreditoIdSeq = "planos_faturamento_creditos_id_seq";

    public void Configure(EntityTypeBuilder<PlanosCredito> entity)
    {
        entity.ToTable("planos_creditos", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("planos_faturamento_creditos_pkey");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .HasDefaultValueSql($"nextval('faturamento.{PlanosCreditoIdSeq}')");

        entity.Property(e => e.Acima1m)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.049")
            .HasColumnName("acima_1m");

        entity.Property(e => e.Cem250k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.084")
            .HasColumnName("cem_250k");

        entity.Property(e => e.Cinco10k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.141")
            .HasColumnName("cinco_10k");

        entity.Property(e => e.Cinquenta100k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.098")
            .HasColumnName("cinquenta_100k");

        entity.Property(e => e.Dez25k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.127")
            .HasColumnName("dez_25k");

        entity.Property(e => e.Dois5k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.162")
            .HasColumnName("dois_5k");

        entity.Property(e => e.DuzentosCinquenta500k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.070")
            .HasColumnName("duzentos_cinquenta_500k");

        entity.Property(e => e.IsDefault)
            .HasDefaultValue(false)
            .HasColumnName("is_default");

        entity.Property(e => e.Nome)
            .HasColumnType("citext")
            .HasColumnName("nome");

        entity.Property(e => e.Quinhentos1m)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.056")
            .HasColumnName("quinhentos_1m");

        entity.Property(e => e.VinteCinco50k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.112")
            .HasColumnName("vinte_cinco_50k");

        entity.Property(e => e.Zero2k)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("0.190")
            .HasColumnName("zero_2k");
    }
}
