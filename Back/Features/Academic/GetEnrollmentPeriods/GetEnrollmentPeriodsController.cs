namespace Syki.Back.Features.Academic.GetEnrollmentPeriods;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetEnrollmentPeriodsController(GetEnrollmentPeriodsService service) : ControllerBase
{
    /// <summary>
    /// Períodos de matrícula
    /// </summary>
    /// <remarks>
    /// Retorna todos os períodos de matrícula.
    /// </remarks>
    [HttpGet("academic/enrollment-periods")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}
