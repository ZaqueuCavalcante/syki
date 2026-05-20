namespace Syki.Back.Features.Courses.CreateCourse;

[ApiController, Authorize(Policies.CreateCourse)]
public class CreateCourseController(CreateCourseService service) : ControllerBase
{
    /// <summary>
    /// Criar curso
    /// </summary>
    /// <remarks>
    /// Cria um novo curso.
    /// </remarks>
    [HttpPost("courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCourseIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCourseIn>;
internal class ResponseExamples : ExamplesProvider<CreateCourseOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidCourseName,
    InvalidCourseType
>;
