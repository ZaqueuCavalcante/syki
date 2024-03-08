namespace Syki.Back.GetCampi;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetCampiController(GetCampiService service) : ControllerBase
{
    [HttpGet("campi")]
    public async Task<IActionResult> Get()
    {
        var campi = await service.Get(User.InstitutionId());

        return Ok(campi);
    }
}
