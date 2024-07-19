namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

/// <summary>
/// Cria um novo período acadêmico.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : SykiController
{
    [HttpPost("academic/academic-periods")]
    [ProducesResponseType(typeof(AcademicPeriodOut), 200)]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
