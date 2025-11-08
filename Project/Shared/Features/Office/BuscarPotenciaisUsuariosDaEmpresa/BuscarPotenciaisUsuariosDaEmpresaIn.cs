namespace Exato.Shared.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

public class BuscarPotenciaisUsuariosDaEmpresaIn : IApiDto<BuscarPotenciaisUsuariosDaEmpresaIn>
{
    public string? NameOuEmail { get; set; }

    public static IEnumerable<(string, BuscarPotenciaisUsuariosDaEmpresaIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarPotenciaisUsuariosDaEmpresaIn() { }),
    ];
}
