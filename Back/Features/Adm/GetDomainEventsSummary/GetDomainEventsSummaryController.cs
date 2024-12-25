namespace Syki.Back.Features.Adm.GetDomainEventsSummary;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetDomainEventsSummaryController(GetDomainEventsSummaryService service) : ControllerBase
{
    /// <summary>
    /// Eventos de domínio
    /// </summary>
    /// <remarks>
    /// Retorna dados consolidados sobre os eventos de domínio.
    /// </remarks>
    [HttpGet("adm/domain-events/summary")]
    public async Task<IActionResult> Get()
    {
        var events = await service.Get();

        return Ok(events);
    }
}
