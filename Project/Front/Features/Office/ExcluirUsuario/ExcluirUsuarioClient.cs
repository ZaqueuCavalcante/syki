using Exato.Shared.Features.Office.ExcluirUsuario;

namespace Exato.Front.Features.Office.ExcluirUsuario;

public class ExcluirUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<ExcluirUsuarioOut, ErrorOut>> Excluir(int id)
    {
        var response = await http.PutAsJsonAsync($"office/usuarios/{id}/excluir", new {});

        return await response.Resolve<ExcluirUsuarioOut>();
    }
}
