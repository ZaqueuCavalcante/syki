namespace Exato.Shared.Features.Office.BuscarUsuariosDaEmpresa;

public class BuscarUsuariosDaEmpresaOut : IApiDto<BuscarUsuariosDaEmpresaOut>
{
    public int Total { get; set; }
    public List<BuscarUsuariosDaEmpresaItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarUsuariosDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarUsuariosDaEmpresaOut() { }),
    ];
}

public class BuscarUsuariosDaEmpresaItemOut
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Email { get; set; }
    public string Documento { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
}
