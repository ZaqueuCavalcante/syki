namespace Exato.Shared.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

public class BuscarPotenciaisUsuariosDaEmpresaOut : IApiDto<BuscarPotenciaisUsuariosDaEmpresaOut>
{
    public List<BuscarPotenciaisUsuariosDaEmpresaItemOut> Items { get; set; } = [];

    public static IEnumerable<(string, BuscarPotenciaisUsuariosDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarPotenciaisUsuariosDaEmpresaOut() { }),
    ];
}

public class BuscarPotenciaisUsuariosDaEmpresaItemOut
{
    public int Id { get; set; }
    public bool Active { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
