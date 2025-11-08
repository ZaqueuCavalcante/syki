namespace Exato.Shared.Features.Office.VincularEmpresasAoUsuario;

public class VincularEmpresasAoUsuarioOut : IApiDto<VincularEmpresasAoUsuarioOut>
{
    public static IEnumerable<(string, VincularEmpresasAoUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new VincularEmpresasAoUsuarioOut()),
    ];
}
