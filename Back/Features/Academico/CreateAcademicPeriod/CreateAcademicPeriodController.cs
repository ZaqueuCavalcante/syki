namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

/// <summary>
/// Cria um novo período acadêmico.
/// </summary>
[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : ControllerBase
{
    [HttpPost("academic-periods")]
    [ProducesResponseType(typeof(AcademicPeriodOut), 200)]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var period = await service.Create(User.InstitutionId(), data);

        return Ok(period);
    }
}
