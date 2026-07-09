namespace Estud.Back.Features.Periods.GetEnrollmentPeriods;

[ApiController, Authorize(Policies.GetEnrollmentPeriods)]
public class GetEnrollmentPeriods(GetEnrollmentPeriodsService service) : ControllerBase
{
    /// <summary>
    /// Períodos de matrícula
    /// </summary>
    /// <remarks>
    /// Retorna todos os períodos de matrícula.
    /// </remarks>
    [HttpGet("periods/enrollment")]
    public async Task<IActionResult> Get()
    {
        var periods = await service.Get();
        return Ok(periods);
    }
}
