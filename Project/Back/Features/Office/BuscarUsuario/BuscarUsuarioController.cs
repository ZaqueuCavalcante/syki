using Exato.Shared.Features.Office.BuscarUsuario;

namespace Exato.Back.Features.Office.BuscarUsuario;

[ApiController, Authorize(Policies.BuscarUsuario)]
public class BuscarUsuarioController(BuscarUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Usuário
    /// </summary>
    /// <remarks>
    /// Retorna o usuário.
    /// </remarks>
    [HttpGet("office/usuarios/{id:int}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarUsuarioOut>;
