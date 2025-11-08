namespace Exato.Shared.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

public class BuscarPotenciaisEmpresasDoUsuarioOut : IApiDto<BuscarPotenciaisEmpresasDoUsuarioOut>
{
    public List<BuscarPotenciaisEmpresasDoUsuarioItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarPotenciaisEmpresasDoUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarPotenciaisEmpresasDoUsuarioOut() { }),
    ];
}

public class BuscarPotenciaisEmpresasDoUsuarioItemOut
{
    public int Id { get; set; }
    public bool Ativa { get; set; }
    public string CNPJ { get; set; }
    public string Nome { get; set; }
    public TipoDeEmpresa Tipo { get; set; }
}
