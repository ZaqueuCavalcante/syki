namespace Exato.Shared.Features.Office.VincularEmpresaUsuario;

public class VincularEmpresaUsuarioIn : IApiDto<VincularEmpresaUsuarioIn>
{
    public int ClienteId { get; set; }
    public int UserId { get; set; }

    public static IEnumerable<(string, VincularEmpresaUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new VincularEmpresaUsuarioIn() { }),
    ];
}
