namespace Syki.Back.Features.Academic.CreateEnrollmentPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateEnrollmentPeriodController(CreateEnrollmentPeriodService service) : ControllerBase
{
    /// <summary>
    /// Criar período de matrícula
    /// </summary>
    /// <remarks>
    /// Cria um novo período de matrícula.
    /// </remarks>
    [HttpPost("academic/enrollment-periods")]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentPeriodIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
