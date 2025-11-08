namespace Exato.Shared.Features.Office.EditarSaldoDaEmpresa;

public class EditarSaldoDaEmpresaOut : IApiDto<EditarSaldoDaEmpresaOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarSaldoDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new EditarSaldoDaEmpresaOut() { }),
    ];
}
