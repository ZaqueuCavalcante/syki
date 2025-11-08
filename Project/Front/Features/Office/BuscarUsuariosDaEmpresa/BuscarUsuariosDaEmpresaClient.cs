using Exato.Shared.Features.Office.BuscarUsuariosDaEmpresa;

namespace Exato.Front.Features.Office.BuscarUsuariosDaEmpresa;

public class BuscarUsuariosDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarUsuariosDaEmpresaOut> Get(int id, BuscarUsuariosDaEmpresaIn data)
    {
        return await http.GetFromJsonAsync<BuscarUsuariosDaEmpresaOut>($"office/empresas/{id}/usuarios".AddQueryString(data)) ?? new();
    }
}
