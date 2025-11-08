using Exato.Shared.Features.Office.BuscarTiposDeResultado;

namespace Exato.Back.Features.Office.BuscarTiposDeResultado;

[ApiController, Authorize(Policies.BuscarTiposDeResultado)]
public class BuscarTiposDeResultadoController(BuscarTiposDeResultadoService service) : ControllerBase
{
    /// <summary>
    /// Tipos de resultado
    /// </summary>
    /// <remarks>
    /// Retorna os tipos de resultado de uma consulta.
    /// </remarks>
    [HttpGet("office/tipos-de-resultado")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] BuscarTiposDeResultadoIn data)
    {
        var tipos = await service.Get(data);
        return Ok(tipos);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarTiposDeResultadoOut>;
