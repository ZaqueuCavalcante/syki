namespace Estud.Back.Features.Courses.RemoveCourseDiscipline;

[ApiController, Authorize(Policies.RemoveCourseDiscipline)]
public class RemoveCourseDisciplineController(RemoveCourseDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Desvincular disciplina de curso
    /// </summary>
    /// <remarks>
    /// Remove o vínculo entre uma disciplina e um curso da instituição.
    /// </remarks>
    [HttpDelete("courses/disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Remove([FromBody] RemoveCourseDisciplineIn data)
    {
        var result = await service.Remove(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<RemoveCourseDisciplineIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseNotFound,
    CourseDisciplineNotFound
>;
