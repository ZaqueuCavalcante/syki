namespace Syki.Back.Features.Student.GetCurrentEnrollmentPeriod;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetEnrollmentPeriodsController(GetCurrentEnrollmentPeriodService service) : ControllerBase
{
    /// <summary>
    /// Período de matrícula atual
    /// </summary>
    /// <remarks>
    /// Retorna o período de matrícula atual, caso o request seja feito durante a vigência do mesmo.
    /// </remarks>
    [HttpGet("student/enrollment-periods/current")]
    public async Task<IActionResult> Get()
    {
        var period = await service.Get(User.InstitutionId());

        return Ok(period);
    }
}
