using Exato.Shared.Features.Office.BuscarConsulta;

namespace Exato.Front.Features.Office.BuscarConsulta;

public class BuscarConsultaClient(HttpClient http) : IOfficeClient
{
    public async Task<BuscarConsultaOut> Buscar(string uid)
    {
        return await http.GetFromJsonAsync<BuscarConsultaOut>($"office/consultas/{uid}") ?? new();
    }
}
