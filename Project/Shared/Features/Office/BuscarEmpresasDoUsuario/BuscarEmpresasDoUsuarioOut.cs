namespace Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

public class BuscarEmpresasDoUsuarioOut : IApiDto<BuscarEmpresasDoUsuarioOut>
{
    public int Total { get; set; }
    public List<BuscarEmpresasDoUsuarioItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarEmpresasDoUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarEmpresasDoUsuarioOut() { }),
    ];
}

public class BuscarEmpresasDoUsuarioItemOut
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public string CNPJ { get; set; }
    public string Nome { get; set; }
    public TipoDeEmpresa Tipo { get; set; }
}
