namespace Syki.Back.Features.Adm.GetCommands;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetCommandsController(GetCommandsService service) : ControllerBase
{
    /// <summary>
    /// Comandos
    /// </summary>
    /// <remarks>
    /// Retorna todos os comandos.
    /// </remarks>
    [HttpGet("adm/commands")]
    public async Task<IActionResult> Get([FromQuery] CommandTableFilterIn filters)
    {
        var commands = await service.Get(filters);

        return Ok(commands);
    }
}
