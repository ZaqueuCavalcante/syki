namespace Exato.Shared.Features.Office.EditarCadastroDaEmpresa;

public class EditarCadastroDaEmpresaOut : IApiDto<EditarCadastroDaEmpresaOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarCadastroDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new EditarCadastroDaEmpresaOut() { }),
    ];
}
