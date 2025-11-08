using Exato.Shared.Features.Office.DesvincularEmpresaUsuario;

namespace Exato.Front.Features.Office.DesvincularEmpresaUsuario;

public class DesvincularEmpresaUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<DesvincularEmpresaUsuarioOut, ErrorOut>> Desvincular(
        int clienteId,
        int userId)
    {
        var body = new DesvincularEmpresaUsuarioIn { ClienteId = clienteId, UserId = userId };

        var response = await http.PutAsJsonAsync($"office/empresa-usuario", body);

        return await response.Resolve<DesvincularEmpresaUsuarioOut>();
    }
}
