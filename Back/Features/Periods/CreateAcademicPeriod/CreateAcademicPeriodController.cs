namespace Syki.Back.Features.Periods.CreateAcademicPeriod;

[ApiController, Authorize(Policies.CreateAcademicPeriod)]
public class CreateAcademicPeriodController(CreateAcademicPeriodService service) : ControllerBase
{
    /// <summary>
    /// Criar período acadêmico
    /// </summary>
    /// <remarks>
    /// Cria um novo período acadêmico.
    /// </remarks>
    [HttpPost("periods/academic")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateAcademicPeriodIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateAcademicPeriodIn> { }
internal class ResponseExamples : ExamplesProvider<CreateAcademicPeriodOut> { }
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidAcademicPeriod,
    InvalidAcademicPeriodDates,
    AcademicPeriodAlreadyExists,
    InvalidAcademicPeriodEndDate,
    InvalidAcademicPeriodStartDate
>;
