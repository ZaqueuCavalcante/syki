using Exato.Shared.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

namespace Exato.Back.Features.Office.BuscarConfiguracoesDeFaturamentoDaEmpresa;

[ApiController, Authorize(Policies.BuscarConfiguracoesDeFaturamentoDaEmpresa)]
public class BuscarConfiguracoesDeFaturamentoDaEmpresaController(BuscarConfiguracoesDeFaturamentoDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Configurações de faturamento da empresa
    /// </summary>
    /// <remarks>
    /// Retorna as configurações de faturamento da empresa.
    /// </remarks>
    [HttpGet("office/empresas/{id:int}/faturamento/configs")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var configs = await service.Get(id);
        return Ok(configs);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarConfiguracoesDeFaturamentoDaEmpresaOut>;
