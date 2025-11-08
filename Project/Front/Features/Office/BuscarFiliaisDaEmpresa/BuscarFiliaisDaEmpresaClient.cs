using Exato.Shared.Features.Office.BuscarFiliaisDaEmpresa;

namespace Exato.Front.Features.Office.BuscarFiliaisDaEmpresa;

public class BuscarFiliaisDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarFiliaisDaEmpresaOut> Get(int id)
    {
        return await http.GetFromJsonAsync<BuscarFiliaisDaEmpresaOut>($"office/empresas/{id}/filiais") ?? new();
    }
}
