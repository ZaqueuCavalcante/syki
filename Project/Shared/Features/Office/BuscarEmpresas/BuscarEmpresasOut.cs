namespace Exato.Shared.Features.Office.BuscarEmpresas;

public class BuscarEmpresasOut : IApiDto<BuscarEmpresasOut>
{
    public int Total { get; set; }
    public List<BuscarEmpresasItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarEmpresasOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarEmpresasOut() { }),
    ];
}

public class BuscarEmpresasItemOut
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public string CNPJ { get; set; }
    public string Nome { get; set; }
    public string RazaoSocial { get; set; }
    public DateTime CriadaEm { get; set; }
    public TipoDeEmpresa Tipo { get; set; }
    public MetodoDePagamento MetodoDePagamento { get; set; }
}
