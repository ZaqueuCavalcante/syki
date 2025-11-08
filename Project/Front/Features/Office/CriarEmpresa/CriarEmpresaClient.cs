using Exato.Shared.Features.Office.CriarEmpresa;

namespace Exato.Front.Features.Office.CriarEmpresa;

public class CriarEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<CriarEmpresaOut, ErrorOut>> Create(string nome, string cnpj, string razaoSocial, bool exatoWeb)
    {
        var body = new CriarEmpresaIn { Nome = nome, CNPJ = cnpj, RazaoSocial = razaoSocial, ExatoWeb = exatoWeb };

        var response = await http.PostAsJsonAsync("office/empresas", body);

        return await response.Resolve<CriarEmpresaOut>();
    }
}
