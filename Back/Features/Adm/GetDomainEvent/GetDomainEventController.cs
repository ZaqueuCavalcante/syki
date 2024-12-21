namespace Syki.Back.Features.Adm.GetDomainEvent;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetDomainEventController(GetDomainEventService service) : ControllerBase
{
    /// <summary>
    /// Evento de domínio
    /// </summary>
    /// <remarks>
    /// Retorna o evento de domínio especificado.
    /// </remarks>
    [HttpGet("adm/domain-events/{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var evt = await service.Get(id);

        return Ok(evt);
    }
}
