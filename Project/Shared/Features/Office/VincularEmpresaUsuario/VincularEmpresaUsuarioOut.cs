namespace Exato.Shared.Features.Office.VincularEmpresaUsuario;

public class VincularEmpresaUsuarioOut : IApiDto<VincularEmpresaUsuarioOut>
{
    public static IEnumerable<(string, VincularEmpresaUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new VincularEmpresaUsuarioOut()),
    ];
}
