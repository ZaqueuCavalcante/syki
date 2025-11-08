using Exato.Shared.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

namespace Exato.Front.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

public class BuscarPotenciaisUsuariosDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarPotenciaisUsuariosDaEmpresaOut> Get(int id, BuscarPotenciaisUsuariosDaEmpresaIn data)
    {
        var path = $"office/empresas/{id}/potenciais-usuarios".AddQueryString(data);
        return await http.GetFromJsonAsync<BuscarPotenciaisUsuariosDaEmpresaOut>(path, HttpConfigs.JsonOptions) ?? new();
    }
}
