namespace Syki.Back.Features.Academic.FinalizeClasses;

[ApiController, Authorize]
[EnableRateLimiting("Medium")]
public class FinalizeClassesController(FinalizeClassesService service) : ControllerBase
{
    /// <summary>
    /// Finalizar turmas
    /// </summary>
    /// <remarks>
    /// Finaliza várias turmas.
    /// </remarks>
    [HttpPut("academic/classes/finalize")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Finalize([FromBody] FinalizeClassesIn data)
    {
        var result = await service.Finalize(User.InstitutionId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<FinalizeClassesIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidClassesList,
    ClassMustHaveStartedStatus,
    AllClassLessonsMustHaveFinalizedStatus>;
