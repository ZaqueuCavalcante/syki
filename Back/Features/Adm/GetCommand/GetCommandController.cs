namespace Syki.Back.Features.Adm.GetCommand;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetCommandController(GetCommandService service) : ControllerBase
{
    /// <summary>
    /// Comando
    /// </summary>
    /// <remarks>
    /// Retorna o comando especificado.
    /// </remarks>
    [HttpGet("adm/commands/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var command = await service.Get(id);

        return Ok(command);
    }
}
