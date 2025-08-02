namespace Syki.Back.Features.Academic.ReleaseClassesForEnrollment;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class ReleaseClassesForEnrollmentController(ReleaseClassesForEnrollmentService service) : ControllerBase
{
    /// <summary>
    /// Liberar turmas
    /// </summary>
    /// <remarks>
    /// Libera as turmas informadas para que os alunos possam se matricular.
    /// </remarks>
    [HttpPut("academic/classes/release-for-enrollment")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Release([FromBody] ReleaseClassesForEnrollmentIn data)
    {
        var result = await service.Release(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<ReleaseClassesForEnrollmentIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    EnrollmentPeriodNotFound,
    EnrollmentPeriodNotStarted,
    EnrollmentPeriodFinalized,
    AllClassesMustHaveOnPreEnrollmentStatus>;
