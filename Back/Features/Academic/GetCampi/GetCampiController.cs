namespace Syki.Back.Features.Academic.GetCampi;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCampiController(GetCampiService service) : ControllerBase
{
    /// <summary>
    /// Campi
    /// </summary>
    /// <remarks>
    /// Retorna todos os campus da insitituição.
    /// </remarks>
    [HttpGet("academic/campi")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var campi = await service.Get(User.InstitutionId());

        return Ok(campi);
    }
}
