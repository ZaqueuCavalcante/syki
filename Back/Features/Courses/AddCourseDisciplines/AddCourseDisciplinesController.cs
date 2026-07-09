namespace Estud.Back.Features.Courses.AddCourseDisciplines;

[ApiController, Authorize(Policies.AddCourseDisciplines)]
public class AddCourseDisciplinesController(AddCourseDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Vincular curso a disciplinas
    /// </summary>
    /// <remarks>
    /// Vincula um curso existente a uma ou mais disciplinas da instituição.
    /// </remarks>
    [HttpPost("courses/disciplines")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Add([FromBody] AddCourseDisciplinesIn data)
    {
        var result = await service.Add(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<AddCourseDisciplinesIn>;
internal class ResponseExamples : ExamplesProvider<SuccessOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseNotFound,
    InvalidDisciplinesList
>;
