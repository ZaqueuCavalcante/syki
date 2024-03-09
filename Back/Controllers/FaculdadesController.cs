namespace Syki.Back.Controllers;

[ApiController, AuthAdm]
[EnableRateLimiting("Medium")]
public class FaculdadesController(IFaculdadesService service) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] FaculdadeIn data)
    {
        var faculdade = await service.Create(data);

        return Ok(faculdade);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var faculdades = await service.GetAll();

        return Ok(faculdades);
    }
}
