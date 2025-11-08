using Exato.Shared.Features.Office.BuscarUsuarios;

namespace Exato.Back.Features.Office.BuscarUsuarios;

[ApiController, Authorize(Policies.BuscarUsuarios)]
public class BuscarUsuariosController(BuscarUsuariosService service) : ControllerBase
{
    /// <summary>
    /// Usuários
    /// </summary>
    /// <remarks>
    /// Retorna os usuários.
    /// </remarks>
    [HttpGet("office/usuarios")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] BuscarUsuariosIn data)
    {
        var usuarios = await service.Get(data);
        return Ok(usuarios);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarUsuariosOut>;
