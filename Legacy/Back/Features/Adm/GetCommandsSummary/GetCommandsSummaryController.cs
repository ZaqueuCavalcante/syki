namespace Estud.Back.Features.Adm.GetCommandsSummary;

[ApiController, Authorize]
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
