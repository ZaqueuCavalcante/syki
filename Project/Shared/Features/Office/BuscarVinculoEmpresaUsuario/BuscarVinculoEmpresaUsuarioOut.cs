namespace Exato.Shared.Features.Office.BuscarVinculoEmpresaUsuario;

public class BuscarVinculoEmpresaUsuarioOut : IApiDto<BuscarVinculoEmpresaUsuarioOut>
{
    /// <summary>
    /// Se o vínculo é utilizado pelo Dexter para realizar consultas.
    /// </summary>
    public bool UsedByDexter { get; set; }

    public static IEnumerable<(string, BuscarVinculoEmpresaUsuarioOut)> GetExamples() =>
    [
        ("Exemplo", new BuscarVinculoEmpresaUsuarioOut()),
    ];
}
