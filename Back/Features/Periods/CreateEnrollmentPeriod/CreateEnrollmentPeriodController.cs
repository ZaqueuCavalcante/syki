namespace Estud.Back.Features.Periods.CreateEnrollmentPeriod;

[ApiController, Authorize(Policies.CreateEnrollmentPeriod)]
public class CreateEnrollmentPeriodController(CreateEnrollmentPeriodService service) : ControllerBase
{
    /// <summary>
    /// Criar período de matrícula
    /// </summary>
    /// <remarks>
    /// Cria um novo período de matrícula.
    /// </remarks>
    [HttpPost("periods/enrollment")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentPeriodIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateEnrollmentPeriodIn> { }
internal class ResponseExamples : ExamplesProvider<CreateEnrollmentPeriodOut> { }
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidEnrollmentPeriodDates,
    EnrollmentPeriodAlreadyExists
>;
