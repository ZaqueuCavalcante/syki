namespace Exato.Back.Features.Office.GetCommand;

[ApiController, Authorize(Policies.GetCommand)]
public class GetCommandController(GetCommandService service) : ControllerBase
{
    /// <summary>
    /// Comando
    /// </summary>
    /// <remarks>
    /// Retorna o comando espeficicado.
    /// </remarks>
    [HttpGet("office/commands/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
