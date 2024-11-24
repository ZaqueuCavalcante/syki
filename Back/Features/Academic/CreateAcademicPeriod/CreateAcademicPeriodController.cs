namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : ControllerBase
{
    /// <summary>
    /// Criar período acadêmico
    /// </summary>
    /// <remarks>
    /// Cria um novo período acadêmico.
    /// </remarks>
    [HttpPost("academic/academic-periods")]
    [ProducesResponseType(typeof(AcademicPeriodOut), 200)]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
