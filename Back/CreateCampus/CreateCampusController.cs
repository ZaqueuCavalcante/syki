namespace Syki.Back.CreateCampus;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateCampusController(CreateCampusService service) : ControllerBase
{
    [HttpPost("campi")]
    public async Task<IActionResult> Create([FromBody] CreateCampusIn data)
    {
        var campus = await service.Create(User.InstitutionId(), data);

        return Ok(campus);
    }
}
