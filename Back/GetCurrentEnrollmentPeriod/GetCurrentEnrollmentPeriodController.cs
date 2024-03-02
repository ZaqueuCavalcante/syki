namespace Syki.Back.GetCurrentEnrollmentPeriod;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetEnrollmentPeriodsController(GetCurrentEnrollmentPeriodService service) : ControllerBase
{
    [HttpGet("enrollment-periods/current")]
    public async Task<IActionResult> Get()
    {
        var period = await service.Get(User.InstitutionId());

        return Ok(period);
    }
}
