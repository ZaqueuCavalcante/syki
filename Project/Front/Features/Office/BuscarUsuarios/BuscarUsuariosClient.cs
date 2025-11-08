using Exato.Shared.Features.Office.BuscarUsuarios;

namespace Exato.Front.Features.Office.BuscarUsuarios;

public class BuscarUsuariosClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarUsuariosOut> Get(BuscarUsuariosIn data)
    {
        return await http.GetFromJsonAsync<BuscarUsuariosOut>("office/usuarios".AddQueryString(data)) ?? new();
    }
}
