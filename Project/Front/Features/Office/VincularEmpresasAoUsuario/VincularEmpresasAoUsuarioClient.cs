using Exato.Shared.Features.Office.VincularEmpresasAoUsuario;

namespace Exato.Front.Features.Office.VincularEmpresasAoUsuario;

public class VincularEmpresasAoUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<VincularEmpresasAoUsuarioOut, ErrorOut>> Vincular(
        int userId,
        List<int> empresas)
    {
        var body = new VincularEmpresasAoUsuarioIn { Empresas = empresas ?? [] };

        var response = await http.PostAsJsonAsync($"office/usuarios/{userId}/empresas", body);

        return await response.Resolve<VincularEmpresasAoUsuarioOut>();
    }
}
