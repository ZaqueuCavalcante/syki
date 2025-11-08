namespace Exato.Shared.Features.Office.EditarConsultasDaEmpresa;

public class EditarConsultasDaEmpresaOut : IApiDto<EditarConsultasDaEmpresaOut>
{
    public int Id { get; set; }

    public static IEnumerable<(string, EditarConsultasDaEmpresaOut)> GetExamples() =>
    [
        ("Exemplo", new EditarConsultasDaEmpresaOut() { }),
    ];
}
