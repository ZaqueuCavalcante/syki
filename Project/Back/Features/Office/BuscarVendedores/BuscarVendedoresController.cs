using Exato.Shared.Features.Office.BuscarVendedores;

namespace Exato.Back.Features.Office.BuscarVendedores;

[ApiController, Authorize(Policies.BuscarVendedores)]
public class BuscarVendedoresController(BuscarVendedoresService service) : ControllerBase
{
    /// <summary>
    /// Vendedores
    /// </summary>
    /// <remarks>
    /// Retorna os vendedores.
    /// </remarks>
    [HttpGet("office/vendedores")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var vendedores = await service.Get();
        return Ok(vendedores);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarVendedoresOut>;
