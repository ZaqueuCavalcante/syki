using Exato.Shared.Features.Office.GetCommands;

namespace Exato.Back.Features.Office.GetCommands;

[ApiController, Authorize(Policies.GetCommands)]
public class GetCommandsController(GetCommandsService service) : ControllerBase
{
    /// <summary>
    /// Comandos
    /// </summary>
    /// <remarks>
    /// Retorna os comandos.
    /// </remarks>
    [HttpGet("office/commands")]
    public async Task<IActionResult> Get([FromQuery] GetCommandsIn data)
    {
        var commands = await service.Get(data);
        return Ok(commands);
    }
}
