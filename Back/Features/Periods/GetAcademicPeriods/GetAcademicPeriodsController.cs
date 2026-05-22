namespace Syki.Back.Features.Periods.GetAcademicPeriods;

[ApiController, Authorize(Policies.GetAcademicPeriods)]
public class GetAcademicPeriods(GetAcademicPeriodsService service) : ControllerBase
{
    /// <summary>
    /// Períodos acadêmicos
    /// </summary>
    /// <remarks>
    /// Retorna todos os períodos acadêmicos.
    /// </remarks>
    [HttpGet("periods/academic")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get();
        return Ok(periods);
    }
}
