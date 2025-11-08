using Exato.Shared.Features.Office.BuscarUsuario;

namespace Exato.Front.Features.Office.BuscarUsuario;

public class BuscarUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<BuscarUsuarioOut, ErrorOut>> Get(int id)
    {
        var response = await http.GetAsync($"office/usuarios/{id}");

        return await response.Resolve<BuscarUsuarioOut>();
    }
}
