namespace Syki.Back.CreateAcademicPeriod;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : ControllerBase
{
    [HttpPost("academic-periods")]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var period = await service.Create(User.InstitutionId(), data);

        return Ok(period);
    }
}
