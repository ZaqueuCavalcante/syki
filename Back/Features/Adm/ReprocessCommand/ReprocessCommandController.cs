namespace Syki.Back.Features.Adm.ReprocessCommand;

[ApiController, AuthAdm]
public class ReprocessCommandController(ReprocessCommandService service) : ControllerBase
{
    /// <summary>
    /// Comando
    /// </summary>
    /// <remarks>
    /// Reprocessa o comando especificado.
    /// </remarks>
    [HttpPost("adm/commands/{id}/reprocess")]
    public async Task<IActionResult> Reprocess([FromRoute] CommandId id)
    {
        var result = await service.Reprocess(id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
