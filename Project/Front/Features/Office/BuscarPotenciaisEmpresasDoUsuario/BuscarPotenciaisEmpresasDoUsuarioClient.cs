using Exato.Shared.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

namespace Exato.Front.Features.Office.BuscarPotenciaisEmpresasDoUsuario;

public class BuscarPotenciaisEmpresasDoUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarPotenciaisEmpresasDoUsuarioOut> Get(int id, BuscarPotenciaisEmpresasDoUsuarioIn data)
    {
        var path = $"office/usuarios/{id}/potenciais-empresas".AddQueryString(data);
        return await http.GetFromJsonAsync<BuscarPotenciaisEmpresasDoUsuarioOut>(path, HttpConfigs.JsonOptions) ?? new();
    }
}
