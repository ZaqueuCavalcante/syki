using Exato.Back.Intelligence.Domain.Faturamento;
using Exato.Back.Intelligence.Configs.Faturamento;

namespace Exato.Back.Database;

public partial class BackDbContext
{
    public DbSet<ClienteConfig> FaturamentoClienteConfig { get; set; }

    // TODO: Scaffold this
    // - >> faturamento.consumo_creditos
    // - >> faturamento.consumo_relatorios

    private static void ConfigureFaturamentoSchema(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ClienteContactDbConfig());

        modelBuilder.ApplyConfiguration(new ClientePlanosRelatorioDbConfig());

        modelBuilder.ApplyConfiguration(new PlanoDbConfig());

        modelBuilder.ApplyConfiguration(new ClienteConfigDbConfig());
        modelBuilder.HasSequence(ClienteConfigDbConfig.ClienteConfigIdSeq, "faturamento");

        modelBuilder.ApplyConfiguration(new PlanosCreditoDbConfig());
        modelBuilder.HasSequence(PlanosCreditoDbConfig.PlanosCreditoIdSeq, "faturamento");

        modelBuilder.ApplyConfiguration(new PlanosRelatorioDbConfig());
        modelBuilder.HasSequence(PlanosRelatorioDbConfig.PlanosRelatorioIdSeq, "faturamento");

        modelBuilder.ApplyConfiguration(new PrecificacaoDbConfig());

        modelBuilder.ApplyConfiguration(new ValorTotalDbConfig());
        modelBuilder.HasSequence(ValorTotalDbConfig.ValorTotalIdSeq, "faturamento");
    }
}
