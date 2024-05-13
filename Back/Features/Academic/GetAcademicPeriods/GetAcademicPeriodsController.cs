namespace Syki.Back.Features.Academic.GetAcademicPeriods;

/// <summary>
/// Retorna todos os períodos acadêmicos.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicPeriods(GetAcademicPeriodsService service) : ControllerBase
{
    [HttpGet("academic/academic-periods")]
    [ProducesResponseType(typeof(List<AcademicPeriodOut>), 200)]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}
