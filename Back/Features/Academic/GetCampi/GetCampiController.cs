namespace Syki.Back.Features.Academic.GetCampi;

/// <summary>
/// Retorna todos os campus da instituição.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCampiController(GetCampiService service) : ControllerBase
{
    [HttpGet("academic/campi")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var campi = await service.Get(User.InstitutionId());

        return Ok(campi);
    }
}
