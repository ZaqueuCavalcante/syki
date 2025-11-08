using Exato.Shared.Features.Office.BuscarConsulta;

namespace Exato.Back.Features.Office.BuscarConsulta;

[ApiController, Authorize(Policies.BuscarConsulta)]
public class BuscarConsultaController(BuscarConsultaService service) : ControllerBase
{
    /// <summary>
    /// Consulta
    /// </summary>
    /// <remarks>
    /// Retorna a consulta especificada.
    /// </remarks>
    [HttpGet("office/consultas/{uid}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] string uid)
    {
        var consulta = await service.Get(uid);
        return Ok(consulta);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarConsultaOut>;
