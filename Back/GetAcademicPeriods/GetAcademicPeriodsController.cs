namespace Syki.Back.GetAcademicPeriods;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetAcademicPeriods(GetAcademicPeriodsService service) : ControllerBase
{
    [HttpGet("academic-periods")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}
