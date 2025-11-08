using Exato.Shared.Features.Office.BuscarEventosDoUsuario;

namespace Exato.Back.Features.Office.BuscarEventosDoUsuario;

[ApiController, Authorize(Policies.BuscarEventosDoUsuario)]
public class BuscarEventosDoUsuarioController(BuscarEventosDoUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Eventos do usuário
    /// </summary>
    /// <remarks>
    /// Retorna os eventos vinculados com o usuário especificado.
    /// </remarks>
    [HttpGet("office/usuarios/{id:int}/eventos")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] BuscarEventosDoUsuarioIn data)
    {
        var eventos = await service.Get(id, data);
        return Ok(eventos);
    }
}

internal class ResponseExamples : ExamplesProvider<BuscarEventosDoUsuarioOut>;
