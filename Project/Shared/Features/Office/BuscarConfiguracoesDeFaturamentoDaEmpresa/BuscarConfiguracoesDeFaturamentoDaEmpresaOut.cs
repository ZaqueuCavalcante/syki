namespace Exato.Shared.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

public class BuscarConfiguracoesDeFaturamentoDaEmpresaOut : IApiDto<BuscarConfiguracoesDeFaturamentoDaEmpresaOut>
{
    public bool IsBillingCustomer { get; set; }
    public int Creditos { get; set; }
    public decimal BalanceInBrl { get; set; }
    public MetodoDePagamento MetodoDePagamento { get; set; }
    public BalanceType BalanceType { get; set; }

    public decimal? FranquiaMinima { get; set; }

    public bool UnmaskedCustomer { get; set; }
    public bool SummaryCustomer { get; set; }
    public bool PreviousCustomer { get; set; }
    public bool FaturamentoPorFaixa { get; set; }
    public bool V1Customer { get; set; }

    public bool FaturamentoPorRateio { get; set; }
    public bool DetalharRelatorios { get; set; }
    public bool ExibirNaoConsumidores { get; set; }

    public static IEnumerable<(string, BuscarConfiguracoesDeFaturamentoDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarConfiguracoesDeFaturamentoDaEmpresaOut() { }),
    ];
}
