using Exato.Shared.Features.Office.BuscarTiposDeConsulta;

namespace Exato.Back.Features.Office.BuscarTiposDeConsulta;

[ApiController, Authorize(Policies.BuscarTiposDeConsulta)]
public class BuscarTiposDeConsultaController(BuscarTiposDeConsultaService service) : ControllerBase
{
    /// <summary>
    /// Tipos de consulta
    /// </summary>
    /// <remarks>
    /// Retorna os tipos de consulta.
    /// </remarks>
    [HttpGet("office/tipos-de-consulta")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] BuscarTiposDeConsultaIn data)
    {
        var tipos = await service.Get(data);
        return Ok(tipos);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarTiposDeConsultaOut>;
