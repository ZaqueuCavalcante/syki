namespace Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

public class BuscarEmpresasDoUsuarioIn : IApiDto<BuscarEmpresasDoUsuarioIn>
{
    public int Page { get; set; }

    public static IEnumerable<(string, BuscarEmpresasDoUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarEmpresasDoUsuarioIn() { }),
    ];
}
