using Exato.Shared.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

namespace Exato.Front.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

public class BuscarPotenciaisMatrizesDaEmpresaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarPotenciaisMatrizesDaEmpresaOut> Get(int id, BuscarPotenciaisMatrizesDaEmpresaIn data)
    {
        var path = $"office/empresas/{id}/potenciais-matrizes".AddQueryString(data);
        return await http.GetFromJsonAsync<BuscarPotenciaisMatrizesDaEmpresaOut>(path, HttpConfigs.JsonOptions) ?? new();
    }
}
