using Exato.Shared.Features.Office.BuscarTiposDeRelatorios;

namespace Exato.Back.Features.Office.BuscarTiposDeRelatorios;

[ApiController, Authorize(Policies.BuscarTiposDeRelatorios)]
public class BuscarTiposDeRelatoriosController(BuscarTiposDeRelatoriosService service) : ControllerBase
{
    /// <summary>
    /// Tipos de relatórios
    /// </summary>
    /// <remarks>
    /// Retorna os tipos de relatórios disponíveis.
    /// </remarks>
    [HttpGet("office/tipos-de-relatorios")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var relatorios = await service.Get();
        return Ok(relatorios);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarTiposDeRelatoriosOut>;
