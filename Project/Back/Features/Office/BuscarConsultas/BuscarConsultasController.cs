using Exato.Shared.Features.Office.BuscarConsultas;

namespace Exato.Back.Features.Office.BuscarConsultas;

[ApiController, Authorize(Policies.BuscarConsultas)]
public class BuscarConsultasController(BuscarConsultasService service) : ControllerBase
{
    /// <summary>
    /// Consultas
    /// </summary>
    /// <remarks>
    /// Retorna as consultas.
    /// </remarks>
    [HttpGet("office/consultas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] BuscarConsultasIn data)
    {
        var consultas = await service.Get(data);
        return Ok(consultas);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarConsultasOut>;
