namespace Syki.Back.Features.Adm.GetCommandsSummary;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetCommandsSummaryController(GetCommandsSummaryService service) : ControllerBase
{
    /// <summary>
    /// Sumário de Comandos
    /// </summary>
    /// <remarks>
    /// Retorna dados consolidados sobre os comandos.
    /// </remarks>
    [HttpGet("adm/commands/summary")]
    public async Task<IActionResult> Get()
    {
        var commands = await service.Get();

        return Ok(commands);
    }
}
