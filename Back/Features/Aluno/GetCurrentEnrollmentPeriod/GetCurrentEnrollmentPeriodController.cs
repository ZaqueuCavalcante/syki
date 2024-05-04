namespace Syki.Back.GetCurrentEnrollmentPeriod;

[ApiController, AuthAluno]
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
