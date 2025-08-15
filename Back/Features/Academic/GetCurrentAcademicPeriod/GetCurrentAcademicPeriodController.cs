namespace Syki.Back.Features.Academic.GetCurrentAcademicPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCurrentAcademicPeriodController(GetCurrentAcademicPeriodService service) : ControllerBase
{
    /// <summary>
    /// Período acadêmico atual
    /// </summary>
    /// <remarks>
    /// Retorna o período acadêmico atual.
    /// </remarks>
    [HttpGet("academic/academic-periods/current")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get()
    {
        var result = await service.Get(User.InstitutionId);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<AcademicPeriodOut>;
internal class ErrorsExamples : ErrorExamplesProvider<CurrentAcademicPeriodNotFound>;
