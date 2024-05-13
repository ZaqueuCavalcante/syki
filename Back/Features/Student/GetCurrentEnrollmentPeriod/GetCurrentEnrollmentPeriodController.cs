namespace Syki.Back.Features.Student.GetCurrentEnrollmentPeriod;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetEnrollmentPeriodsController(GetCurrentEnrollmentPeriodService service) : ControllerBase
{
    [HttpGet("student/enrollment-periods/current")]
    public async Task<IActionResult> Get()
    {
        var period = await service.Get(User.InstitutionId());

        return Ok(period);
    }
}
