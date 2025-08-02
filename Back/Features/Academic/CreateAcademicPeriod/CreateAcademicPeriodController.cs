namespace Syki.Back.Features.Academic.CreateAcademicPeriod;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : ControllerBase
{
    /// <summary>
    /// Criar período acadêmico
    /// </summary>
    /// <remarks>
    /// Cria um novo período acadêmico.
    /// </remarks>
    [HttpPost("academic/academic-periods")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateAcademicPeriodIn> { }

internal class ResponseExamples : ExamplesProvider<AcademicPeriodOut> { }

internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidAcademicPeriod,
    InvalidAcademicPeriodDates,
    AcademicPeriodAlreadyExists,
    InvalidAcademicPeriodEndDate,
    InvalidAcademicPeriodStartDate>
{ }
