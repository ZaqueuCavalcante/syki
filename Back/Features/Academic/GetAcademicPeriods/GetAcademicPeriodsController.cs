namespace Syki.Back.Features.Academic.GetAcademicPeriods;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetAcademicPeriods(GetAcademicPeriodsService service) : ControllerBase
{
    /// <summary>
    /// Períodos acadêmicos
    /// </summary>
    /// <remarks>
    /// Retorna todos os períodos acadêmicos.
    /// </remarks>
    [HttpGet("academic/academic-periods")]
    [ProducesResponseType(typeof(List<AcademicPeriodOut>), 200)]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());

        return Ok(periods);
    }
}
