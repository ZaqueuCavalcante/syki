using Exato.Shared.Features.Office.BuscarEventosDoUsuario;

namespace Exato.Front.Features.Office.BuscarEventosDoUsuario;

public class BuscarEventosDoUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarEventosDoUsuarioOut> Get(int id, BuscarEventosDoUsuarioIn query)
    {
        return await http.GetFromJsonAsync<BuscarEventosDoUsuarioOut>($"office/usuarios/{id}/eventos".AddQueryString(query), HttpConfigs.JsonOptions) ?? new();
    }
}
