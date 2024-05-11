namespace Syki.Back.Features.Academic.CreateCampus;

/// <summary>
/// Cria um novo campus.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCampusController(CreateCampusService service) : ControllerBase
{
    [HttpPost("academic/campi")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCampusIn data)
    {
        var campus = await service.Create(User.InstitutionId(), data);

        return Ok(campus);
    }
}
