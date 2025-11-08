using Exato.Shared.Features.Office.CriarUsuario;

namespace Exato.Front.Features.Office.CriarUsuario;

public class CriarUsuarioClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<CriarUsuarioOut, ErrorOut>> Create(
        string nome,
        string email,
        string? cpf,
        List<ExatoWebClaims> claims)
    {
        var body = new CriarUsuarioIn { Nome = nome, Email = email, Cpf = cpf, Claims = claims };

        var response = await http.PostAsJsonAsync("office/usuarios", body);

        return await response.Resolve<CriarUsuarioOut>();
    }
}
