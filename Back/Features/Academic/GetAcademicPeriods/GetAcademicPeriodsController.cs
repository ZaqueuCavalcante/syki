namespace Syki.Back.Features.Academic.GetAcademicPeriods;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetAcademicPeriods(GetAcademicPeriodsService service) : ControllerBase
{
    /// <summary>
    /// Períodos acadêmicos
    /// </summary>
    /// <remarks>
    /// Retorna todos os períodos acadêmicos.
    /// </remarks>
    [HttpGet("academic/academic-periods")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get(User.InstitutionId());
        return Ok(periods);
    }
}
