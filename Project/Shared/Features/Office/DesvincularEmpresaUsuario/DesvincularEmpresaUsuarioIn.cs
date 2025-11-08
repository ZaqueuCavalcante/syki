namespace Exato.Shared.Features.Office.DesvincularEmpresaUsuario;

public class DesvincularEmpresaUsuarioIn : IApiDto<DesvincularEmpresaUsuarioIn>
{
    public int UserId { get; set; }
    public int ClienteId { get; set; }

    public static IEnumerable<(string, DesvincularEmpresaUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new DesvincularEmpresaUsuarioIn() { }),
    ];
}
