namespace Exato.Shared.Features.Office.BuscarEmpresa;

public class BuscarEmpresaOut : IApiDto<BuscarEmpresaOut>
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public TipoDeEmpresa Tipo { get; set; }
    public string CNPJ { get; set; }
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public int? MatrizId { get; set; }
    public string? Matriz { get; set; }
    public string? NomeFantasia { get; set; }
    public int? QuodSegmentId { get; set; }
    public string? Slug { get; set; }
    public string? SalesContact { get; set; }
    public DateTime CriadaEm { get; set; }

    public bool HighPerformance { get; set; }
    public bool BlockSensitiveDataInQueryString { get; set; }
    public DataAccessLevel DataAccessLevel { get; set; }
    public int? TransLimitPerWeek { get; set; }

    public bool GerarPdfConsultas { get; set; }
    public bool HabilitarConsultasPorEmail { get; set; }

    public bool ReceitaCpfUseSerproAsMainSource { get; set; }
    public bool ReceitaCpfShouldReturnMinor18AgeData { get; set; }

    public BuscarEmpresaRelatorioOut RelatorioPF { get; set; } = new();
    public BuscarEmpresaRelatorioOut RelatorioPFQuod { get; set; } = new();
    public BuscarEmpresaRelatorioOut RelatorioPJ { get; set; } = new();
    public BuscarEmpresaRelatorioOut RelatorioPJQuod { get; set; } = new();

    public bool IsBillingCustomer { get; set; }
    public int Creditos { get; set; }
    public decimal BalanceInBrl { get; set; }
    public MetodoDePagamento MetodoDePagamento { get; set; }
    public BalanceType BalanceType { get; set; }

    public Guid ExternalId { get; set; }
    public bool PossuiFiliais { get; set; }

    public static IEnumerable<(string, BuscarEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarEmpresaOut() { }),
    ];
}

public class BuscarEmpresaRelatorioOut
{
    public int Id { get; set; }
    public string Nome { get; set; }
}
