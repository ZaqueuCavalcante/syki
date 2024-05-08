namespace Syki.Back.CreateEnrollmentPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateEnrollmentPeriodController(CreateEnrollmentPeriodService service) : ControllerBase
{
    [HttpPost("enrollment-periods")]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentPeriodIn data)
    {
        var period = await service.Create(User.InstitutionId(), data);

        return Ok(period);
    }
}
