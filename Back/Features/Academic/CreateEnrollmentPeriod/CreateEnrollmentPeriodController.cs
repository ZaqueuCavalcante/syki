namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateEnrollmentPeriodController(CreateEnrollmentPeriodService service) : ControllerBase
{
    [HttpPost("academic/enrollment-periods")]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentPeriodIn data)
    {
        var period = await service.Create(User.InstitutionId(), data);

        return Ok(period);
    }
}
