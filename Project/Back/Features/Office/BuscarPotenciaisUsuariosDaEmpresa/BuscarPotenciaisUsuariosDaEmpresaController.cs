using Exato.Shared.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

namespace Exato.Back.Features.Office.BuscarPotenciaisUsuariosDaEmpresa;

[ApiController, Authorize(Policies.BuscarPotenciaisUsuariosDaEmpresa)]
public class BuscarPotenciaisUsuariosDaEmpresaController(BuscarPotenciaisUsuariosDaEmpresaService service) : ControllerBase
{
    /// <summary>
    /// Buscar potenciais usuários
    /// </summary>
    /// <remarks>
    /// Busca aos potenciais usuários da empresa informada.
    /// </remarks>
    [HttpGet("office/empresas/{id}/potenciais-usuarios")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] BuscarPotenciaisUsuariosDaEmpresaIn data)
    {
        var usarios = await service.Get(id, data);
        return Ok(usarios);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarPotenciaisUsuariosDaEmpresaOut>;
