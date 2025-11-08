using Exato.Shared.Features.Office.EditarCadastroDaEmpresa;

namespace Exato.Front.Features.Office.EditarCadastroDaEmpresa;

public class EditarCadastroDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<EditarCadastroDaEmpresaOut, ErrorOut>> Editar(
        int id,
        bool ativa,
        string nome,
        string cnpj,
        string razaoSocial,
        int? matrizId,
        string? nomeFantasia,
        string? slug,
        string? salesContact)
    {
        var body = new EditarCadastroDaEmpresaIn
        {
            Ativa = ativa,
            Nome = nome,
            CNPJ = cnpj,
            RazaoSocial = razaoSocial,
            MatrizId = matrizId,
            NomeFantasia = nomeFantasia,
            Slug = slug,
            SalesContact = salesContact,
        };

        var response = await http.PutAsJsonAsync($"office/empresas/{id}/cadastro", body);

        return await response.Resolve<EditarCadastroDaEmpresaOut>();
    }
}
