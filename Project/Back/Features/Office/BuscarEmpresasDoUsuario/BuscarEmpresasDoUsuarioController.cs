using Exato.Shared.Features.Office.BuscarEmpresasDoUsuario;

namespace Exato.Back.Features.Office.BuscarEmpresasDoUsuario;

[ApiController, Authorize(Policies.BuscarEmpresasDoUsuario)]
public class BuscarEmpresasDoUsuarioController(BuscarEmpresasDoUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Empresas do usuário
    /// </summary>
    /// <remarks>
    /// Retorna as empresas vinculadas ao usuário.
    /// </remarks>
    [HttpGet("office/usuarios/{id:int}/empresas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] BuscarEmpresasDoUsuarioIn data)
    {
        var tokens = await service.Get(id, data);
        return Ok(tokens);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarEmpresasDoUsuarioOut>;
