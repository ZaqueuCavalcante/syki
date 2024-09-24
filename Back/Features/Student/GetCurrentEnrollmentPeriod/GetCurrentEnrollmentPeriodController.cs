namespace Syki.Back.Features.Student.GetCurrentEnrollmentPeriod;

/// <summary>
/// Retorna o Período de Matrícula atual, caso o request seja feito durante a vigência do mesmo.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetEnrollmentPeriodsController(GetCurrentEnrollmentPeriodService service) : ControllerBase
{
    [HttpGet("student/enrollment-periods/current")]
    public async Task<IActionResult> Get()
    {
        var period = await service.Get(User.InstitutionId());

        return Ok(period);
    }
}
