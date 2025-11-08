using Exato.Shared.Features.Office.EditarCadastroDoUsuario;

namespace Exato.Front.Features.Office.EditarCadastroDoUsuario;

public class EditarCadastroDoUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarCadastroDoUsuarioOut, ErrorOut>> Editar(
        int id,
        string nome,
        string email,
        string? cpf)
    {
        var body = new EditarCadastroDoUsuarioIn { Nome = nome, Email = email, Cpf = cpf };

        var response = await http.PutAsJsonAsync($"office/usuarios/{id}/cadastro", body);

        return await response.Resolve<EditarCadastroDoUsuarioOut>();
    }
}
