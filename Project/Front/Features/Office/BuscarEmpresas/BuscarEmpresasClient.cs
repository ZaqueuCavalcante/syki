using Exato.Shared.Features.Office.BuscarEmpresas;

namespace Exato.Front.Features.Office.BuscarEmpresas;

public class BuscarEmpresasClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarEmpresasOut> Get(BuscarEmpresasIn data)
    {
        return await http.GetFromJsonAsync<BuscarEmpresasOut>("office/empresas".AddQueryString(data), HttpConfigs.JsonOptions) ?? new();
    }
}
