namespace Syki.Back.Features.Academic.GetEnrollmentPeriods;

/// <summary>
/// Retorna todos os Períodos de Matrícula.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetEnrollmentPeriodsController(GetEnrollmentPeriodsService service) : ControllerBase
{
    [HttpGet("academic/enrollment-periods")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}
