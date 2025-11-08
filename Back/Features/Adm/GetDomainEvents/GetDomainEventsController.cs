namespace Syki.Back.Features.Adm.GetDomainEvents;

[ApiController, Authorize(Policies.GetDomainEvents)]
public class GetDomainEventsController(GetDomainEventsService service) : ControllerBase
{
    /// <summary>
    /// Eventos de domínio
    /// </summary>
    /// <remarks>
    /// Retorna todos os eventos de domínio.
    /// </remarks>
    [HttpGet("adm/domain-events")]
    public async Task<IActionResult> Get([FromQuery] DomainEventTableFilterIn filters)
    {
        var events = await service.Get(filters);

        return Ok(events);
    }
}
