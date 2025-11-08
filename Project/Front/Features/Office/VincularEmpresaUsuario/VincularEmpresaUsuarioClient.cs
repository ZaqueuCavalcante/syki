using Exato.Shared.Features.Office.VincularEmpresaUsuario;

namespace Exato.Front.Features.Office.VincularEmpresaUsuario;

public class VincularEmpresaUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<VincularEmpresaUsuarioOut, ErrorOut>> Vincular(
        int clienteId,
        int userId)
    {
        var body = new VincularEmpresaUsuarioIn { ClienteId = clienteId, UserId = userId };

        var response = await http.PostAsJsonAsync($"office/empresa-usuario", body);

        return await response.Resolve<VincularEmpresaUsuarioOut>();
    }
}
