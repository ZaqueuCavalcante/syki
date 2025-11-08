using Exato.Shared.Features.Office.EditarFaturamentoDaEmpresa;

namespace Exato.Front.Features.Office.EditarFaturamentoDaEmpresa;

public class EditarFaturamentoDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarFaturamentoDaEmpresaOut, ErrorOut>> Editar(
        int id,
        bool habilitado,
        MetodoDePagamento metodoDePagamento)
    {
        var body = new EditarFaturamentoDaEmpresaIn
        {
            Habilitado = habilitado,
            MetodoDePagamento = metodoDePagamento,
        };

        var response = await http.PutAsJsonAsync($"office/empresas/{id}/faturamento", body);

        return await response.Resolve<EditarFaturamentoDaEmpresaOut>();
    }
}
