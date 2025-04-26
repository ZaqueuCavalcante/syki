namespace Syki.Back.Features.Adm.ReprocessCommand;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class ReprocessCommandController(ReprocessCommandService service) : ControllerBase
{
    /// <summary>
    /// Comando
    /// </summary>
    /// <remarks>
    /// Reprocessa o comando especificado.
    /// </remarks>
    [HttpPost("adm/commands/{id:guid}/reprocess")]
    public async Task<IActionResult> Reprocess([FromRoute] Guid id)
    {
        var result = await service.Reprocess(id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
