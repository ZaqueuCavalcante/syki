using Exato.Shared.Features.Office.BuscarTokensDeAcesso;

namespace Exato.Back.Features.Office.BuscarTokensDeAcesso;

[ApiController, Authorize(Policies.BuscarTokensDeAcessoDaEmpresa)]
public class BuscarTokensDeAcessoController(BuscarTokensDeAcessoService service) : ControllerBase
{
    /// <summary>
    /// Tokens
    /// </summary>
    /// <remarks>
    /// Retorna os tokens de acesso.
    /// </remarks>
    [HttpGet("office/tokens")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] BuscarTokensDeAcessoIn data)
    {
        var tokens = await service.Get(data);
        return Ok(tokens);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarTokensDeAcessoOut>;
