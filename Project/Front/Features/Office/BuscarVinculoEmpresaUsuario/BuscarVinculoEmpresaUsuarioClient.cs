using Exato.Shared.Features.Office.BuscarVinculoEmpresaUsuario;

namespace Exato.Front.Features.Office.BuscarVinculoEmpresaUsuario;

public class BuscarVinculoEmpresaUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<BuscarVinculoEmpresaUsuarioOut, ErrorOut>> Buscar(
        int clienteId,
        int userId)
    {
        var body = new BuscarVinculoEmpresaUsuarioIn { ClienteId = clienteId, UserId = userId };

        var response = await http.GetAsync($"office/empresa-usuario".AddQueryString(body));

        return await response.Resolve<BuscarVinculoEmpresaUsuarioOut>();
    }
}
