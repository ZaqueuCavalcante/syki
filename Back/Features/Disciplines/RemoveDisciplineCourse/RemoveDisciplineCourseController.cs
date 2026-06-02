namespace Syki.Back.Features.Disciplines.RemoveDisciplineCourse;

[ApiController, Authorize(Policies.RemoveDisciplineCourse)]
public class RemoveDisciplineCourseController(RemoveDisciplineCourseService service) : ControllerBase
{
    /// <summary>
    /// Desvincular disciplina de curso
    /// </summary>
    /// <remarks>
    /// Remove o vínculo entre uma disciplina e um curso da instituição.
    /// </remarks>
    [HttpDelete("disciplines/courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Remove([FromBody] RemoveDisciplineCourseIn data)
    {
        var result = await service.Remove(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<RemoveDisciplineCourseIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    DisciplineNotFound,
    CourseDisciplineNotFound
>;
