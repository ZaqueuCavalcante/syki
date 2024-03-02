namespace Syki.Back.GetEnrollmentPeriods;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetEnrollmentPeriodsController(GetEnrollmentPeriodsService service) : ControllerBase
{
    [HttpGet("enrollment-periods")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}
