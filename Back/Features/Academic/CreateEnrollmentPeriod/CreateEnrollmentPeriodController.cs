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
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateEnrollmentPeriodIn data)
    {
        var result = await service.Create(User.InstitutionId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateEnrollmentPeriodIn>;
internal class ResponseExamples : ExamplesProvider<EnrollmentPeriodOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    AcademicPeriodNotFound,
    EnrollmentPeriodAlreadyExists,
    InvalidEnrollmentPeriodDates>;
