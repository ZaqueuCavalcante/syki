namespace Syki.Back.Features.Academic.CreateCourse;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateCourseController(CreateCourseService service) : ControllerBase
{
    /// <summary>
    /// Criar curso
    /// </summary>
    /// <remarks>
    /// Cria um novo curso.
    /// </remarks>
    [HttpPost("academic/courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCourseIn data)
    {
        var result = await service.Create(User.InstitutionId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCourseIn>;
internal class ResponseExamples : ExamplesProvider<CourseOut>;
internal class ErrorsExamples : ErrorExamplesProvider<InvalidCourseType>;
