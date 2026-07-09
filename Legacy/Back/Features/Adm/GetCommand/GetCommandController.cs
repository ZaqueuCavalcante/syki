namespace Estud.Back.Features.Adm.GetCommand;

[ApiController, Authorize]
public class GetCommandController(GetCommandService service) : ControllerBase
{
    /// <summary>
    /// Comando
    /// </summary>
    /// <remarks>
    /// Retorna o comando especificado.
    /// </remarks>
    [HttpGet("adm/commands/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var command = await service.Get(id);

        return Ok(command);
    }
}
