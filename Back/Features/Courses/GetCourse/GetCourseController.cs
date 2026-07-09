namespace Estud.Back.Features.Courses.GetCourse;

[ApiController, Authorize(Policies.GetCourse)]
public class GetCourseController(GetCourseService service) : ControllerBase
{
    /// <summary>
    /// Curso
    /// </summary>
    /// <remarks>
    /// Retorna os dados de um curso, incluindo as disciplinas vinculadas.
    /// </remarks>
    [HttpGet("courses/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCourseOut>;
internal class ErrorsExamples : ErrorExamplesProvider<CourseNotFound>;
