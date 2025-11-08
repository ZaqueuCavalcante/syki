namespace Exato.Shared.Features.Office.BuscarVinculoEmpresaUsuario;

public class BuscarVinculoEmpresaUsuarioIn : IApiDto<BuscarVinculoEmpresaUsuarioIn>
{
    public int UserId { get; set; }
    public int ClienteId { get; set; }

    public static IEnumerable<(string, BuscarVinculoEmpresaUsuarioIn)> GetExamples() =>
    [
        ("Exemplo", new BuscarVinculoEmpresaUsuarioIn() { }),
    ];
}
