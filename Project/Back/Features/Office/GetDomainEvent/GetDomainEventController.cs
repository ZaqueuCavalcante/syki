namespace Exato.Back.Features.Office.GetDomainEvent;

[ApiController, Authorize(Policies.GetDomainEvent)]
public class GetDomainEventController(GetDomainEventService service) : ControllerBase
{
    /// <summary>
    /// Evento de domínio
    /// </summary>
    /// <remarks>
    /// Retorna o evento de domínio espeficicado.
    /// </remarks>
    [HttpGet("office/domain-events/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
