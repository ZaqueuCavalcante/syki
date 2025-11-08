using Exato.Shared.Features.Office.BuscarEmpresa;

namespace Exato.Front.Features.Office.BuscarEmpresa;

public class BuscarEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<OneOf<BuscarEmpresaOut, ErrorOut>> Get(int id)
    {
        var response = await http.GetAsync($"office/empresas/{id}");

        return await response.Resolve<BuscarEmpresaOut>();
    }
}
