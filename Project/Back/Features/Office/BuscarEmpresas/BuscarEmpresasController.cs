using Exato.Shared.Features.Office.BuscarEmpresas;

namespace Exato.Back.Features.Office.BuscarEmpresas;

[ApiController, Authorize(Policies.BuscarEmpresas)]
public class BuscarEmpresasController(BuscarEmpresasService service) : ControllerBase
{
    /// <summary>
    /// Empresas
    /// </summary>
    /// <remarks>
    /// Retorna as empresas.
    /// </remarks>
    [HttpGet("office/empresas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] BuscarEmpresasIn data)
    {
        var empresas = await service.Get(data);
        return Ok(empresas);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarEmpresasOut>;
