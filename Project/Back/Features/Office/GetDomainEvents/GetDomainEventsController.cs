using Exato.Shared.Features.Office.GetDomainEvents;

namespace Exato.Back.Features.Office.GetDomainEvents;

[ApiController, Authorize(Policies.GetDomainEvents)]
public class GetDomainEventsController(GetDomainEventsService service) : ControllerBase
{
    /// <summary>
    /// Eventos de domínio
    /// </summary>
    /// <remarks>
    /// Retorna os eventos de domínio.
    /// </remarks>
    [HttpGet("office/domain-events")]
    public async Task<IActionResult> Get([FromQuery] GetDomainEventsIn data)
    {
        var events = await service.Get(data);

        return Ok(events);
    }
}
