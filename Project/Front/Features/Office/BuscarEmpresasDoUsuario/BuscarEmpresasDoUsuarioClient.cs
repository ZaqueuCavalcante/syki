using Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

namespace Exato.Front.Features.Office.BuscarEmpresasDoUsuario;

public class BuscarEmpresasDoUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarEmpresasDoUsuarioOut> Get(int id, BuscarEmpresasDoUsuarioIn query)
    {
        return await http.GetFromJsonAsync<BuscarEmpresasDoUsuarioOut>($"office/usuarios/{id}/empresas".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
