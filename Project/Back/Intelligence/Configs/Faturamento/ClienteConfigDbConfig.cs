using Exato.Back.Intelligence.Domain.Public;
using Exato.Back.Intelligence.Domain.Faturamento;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Exato.Back.Intelligence.Configs.Faturamento;

public class ClienteConfigDbConfig : IEntityTypeConfiguration<ClienteConfig>
{
    public const string ClienteConfigIdSeq = "customers_plan_package_id_seq";

    public void Configure(EntityTypeBuilder<ClienteConfig> entity)
    {
        entity.ToTable("cliente_config", "faturamento");

        entity.HasKey(e => e.Id)
            .HasName("customers_plan_package_pkey");

        entity.Property(e => e.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("id")
            .HasDefaultValueSql($"nextval('faturamento.{ClienteConfigIdSeq}')");

        entity.Property(e => e.BillingPeriodStartDay)
            .HasColumnName("billing_period_start_day");

        entity.Property(e => e.ClienteContactId)
            .HasColumnName("cliente_contact_id");

        entity.Property(e => e.ClienteId)
            .HasColumnName("cliente_id");

        entity.Property(e => e.DetalharRelatorios)
            .HasColumnName("detalhar_relatorios");

        entity.Property(e => e.ExibirNaoConsumidores)
            .HasColumnName("exibir_nao_consumidores");

        entity.Property(e => e.FaturamentoPorFaixa)
            .HasColumnName("faturamento_por_faixa");

        entity.Property(e => e.FaturamentoPorRateio)
            .HasColumnName("faturamento_por_rateio");

        entity.Property(e => e.FranquiaMinima)
            .HasPrecision(10, 3)
            .HasDefaultValueSql("495.000")
            .HasColumnName("franquia_minima");

        entity.Property(e => e.PlanosDoccheckId)
            .HasColumnName("planos_doccheck_id");

        entity.Property(e => e.PreviousCustomer)
            .HasDefaultValue(false)
            .HasColumnName("previous_customer");

        entity.Property(e => e.SummaryCustomer)
            .HasDefaultValue(false)
            .HasColumnName("summary_customer");

        entity.Property(e => e.UnmaskedCustomer)
            .HasDefaultValue(false)
            .HasColumnName("unmasked_customer");

        entity.Property(e => e.V1Customer)
            .HasDefaultValue(false)
            .HasColumnName("v1_customer");

        entity.HasOne<Cliente>()
            .WithMany()
            .HasPrincipalKey(x => x.ClienteId)
            .HasForeignKey(d => d.ClienteId);

        entity.HasOne<ClienteContact>()
            .WithMany()
            .HasPrincipalKey(x => x.Id)
            .HasForeignKey(d => d.ClienteContactId);
    }
}
