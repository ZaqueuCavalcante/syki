using Exato.Shared.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

namespace Exato.Back.Features.Office.BuscarPotenciaisMatrizesDaEmpresa;

[ApiController, Authorize(Policies.BuscarPotenciaisMatrizesDaEmpresa)]
public class BuscarPotenciaisMatrizesDaEmpresaController(BuscarPotenciaisMatrizesDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Buscar potenciais matrizes
    /// </summary>
    /// <remarks>
    /// Busca as potenciais matrizes da empresa informada.
    /// </remarks>
    [HttpGet("office/empresas/{id}/potenciais-matrizes")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] BuscarPotenciaisMatrizesDaEmpresaIn data)
    {
        var matrizes = await service.Get(id, data);
        return Ok(matrizes);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarPotenciaisMatrizesDaEmpresaOut>;
