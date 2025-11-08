namespace Exato.Back.Intelligence.Domain.Faturamento;

/// <summary>
/// Configurações de processamento de faturamento para um cliente.
/// </summary>
public partial class ClienteConfig
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public decimal? FranquiaMinima { get; set; }

    public bool FaturamentoPorFaixa { get; set; }

    public int? PlanosDoccheckId { get; set; }

    public bool FaturamentoPorRateio { get; set; }

    public bool? DetalharRelatorios { get; set; }

    public bool? ExibirNaoConsumidores { get; set; }

    public int? ClienteContactId { get; set; }

    public bool PreviousCustomer { get; set; }

    public bool UnmaskedCustomer { get; set; }

    public bool SummaryCustomer { get; set; }

    public bool V1Customer { get; set; }

    public short? BillingPeriodStartDay { get; set; }

    public ClienteConfig() { }

    public ClienteConfig(int clienteId)
    {
        ClienteId = clienteId;
        FranquiaMinima = 495;
        FaturamentoPorFaixa = false;
        PlanosDoccheckId = 1;
        FaturamentoPorRateio = false;
        DetalharRelatorios = false;
        ExibirNaoConsumidores = false;
    }
}
