namespace Estud.Back.Features.Periods.UpdateEnrollmentPeriod;

[ApiController, Authorize(Policies.UpdateEnrollmentPeriod)]
public class UpdateEnrollmentPeriodController(UpdateEnrollmentPeriodService service) : ControllerBase
{
    /// <summary>
    /// Editar período de matrícula
    /// </summary>
    /// <remarks>
    /// Edita os dados do período de matrícula informado.
    /// </remarks>
    [HttpPut("periods/enrollment")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromBody] UpdateEnrollmentPeriodIn data)
    {
        var result = await service.Update(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateEnrollmentPeriodIn>;
internal class ResponseExamples : ExamplesProvider<UpdateEnrollmentPeriodOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EnrollmentPeriodNotFound,
    EnrollmentPeriodAlreadyExists,
    InvalidEnrollmentPeriodDates
>;
