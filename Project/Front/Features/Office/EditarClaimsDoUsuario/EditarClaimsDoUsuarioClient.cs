using Exato.Shared.Features.Office.EditarClaimsDoUsuario;

namespace Exato.Front.Features.Office.EditarClaimsDoUsuario;

public class EditarClaimsDoUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarClaimsDoUsuarioOut, ErrorOut>> Editar(
        int id,
        List<ExatoWebClaims> claims)
    {
        var body = new EditarClaimsDoUsuarioIn { Claims = claims };

        var response = await http.PutAsJsonAsync($"office/usuarios/{id}/claims", body);

        return await response.Resolve<EditarClaimsDoUsuarioOut>();
    }
}
