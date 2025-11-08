namespace Exato.Shared.Features.Office.DesvincularEmpresaUsuario;

public class DesvincularEmpresaUsuarioOut : IApiDto<DesvincularEmpresaUsuarioOut>
{
    public static IEnumerable<(string, DesvincularEmpresaUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new DesvincularEmpresaUsuarioOut()),
    ];
}
