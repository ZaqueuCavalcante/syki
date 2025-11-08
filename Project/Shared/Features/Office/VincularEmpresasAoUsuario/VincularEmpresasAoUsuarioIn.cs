namespace Exato.Shared.Features.Office.VincularEmpresasAoUsuario;

public class VincularEmpresasAoUsuarioIn : IApiDto<VincularEmpresasAoUsuarioIn>
{
    public List<int> Empresas { get; set; } = [];

    public static IEnumerable<(string, VincularEmpresasAoUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new VincularEmpresasAoUsuarioIn() { }),
    ];
}
