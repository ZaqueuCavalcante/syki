using Exato.Shared.Features.Office.EditarRelatoriosDaEmpresa;

namespace Exato.Front.Features.Office.EditarRelatoriosDaEmpresa;

public class EditarRelatoriosDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarRelatoriosDaEmpresaOut, ErrorOut>> Editar(
        int id,
        int pf,
        int pj,
        int pfQuod,
        int pjQuod)
    {
        var body = new EditarRelatoriosDaEmpresaIn
        {
            Pf = pf,
            Pj = pj,
            PfQuod = pfQuod,
            PjQuod = pjQuod,
        };

        var response = await http.PutAsJsonAsync($"office/empresas/{id}/relatorios", body);

        return await response.Resolve<EditarRelatoriosDaEmpresaOut>();
    }
}
