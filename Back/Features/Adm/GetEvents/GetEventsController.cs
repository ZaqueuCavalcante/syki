namespace Syki.Back.Features.Adm.GetEvents;

[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetEventsController(GetEventsService service) : ControllerBase
{
    /// <summary>
    /// Eventos
    /// </summary>
    /// <remarks>
    /// Retorna todos os eventos.
    /// </remarks>
    [HttpGet("adm/events")]
    public async Task<IActionResult> Get()
    {
        var events = await service.Get();

        return Ok(events);
    }
}
