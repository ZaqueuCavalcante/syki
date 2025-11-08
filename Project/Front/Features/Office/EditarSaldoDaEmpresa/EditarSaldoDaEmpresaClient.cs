using Exato.Shared.Features.Office.EditarSaldoDaEmpresa;

namespace Exato.Front.Features.Office.EditarSaldoDaEmpresa;

public class EditarSaldoDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarSaldoDaEmpresaOut, ErrorOut>> Editar(
        int id,
        decimal amount,
        int credits)
    {
        var body = new EditarSaldoDaEmpresaIn
        {
            Amount = amount,
            Credits = credits,
        };

        var response = await http.PutAsJsonAsync($"office/empresas/{id}/saldo", body);

        return await response.Resolve<EditarSaldoDaEmpresaOut>();
    }
}
