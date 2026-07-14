namespace Estud.Back.Features.Courses.GetCourses;

[ApiController, Authorize(Policies.GetCourses)]
public class GetCoursesController(GetCoursesService service) : ControllerBase
{
    /// <summary>
    /// Cursos
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de cursos da instituição, ordenados por nome.
    /// </remarks>
    [HttpGet("courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetCoursesIn query)
    {
        var courses = await service.Get(query);
        return Ok(courses);
    }
}

internal class RequestExamples : ExamplesProvider<GetCoursesIn>;
internal class ResponseExamples : ExamplesProvider<GetCoursesOut>;
