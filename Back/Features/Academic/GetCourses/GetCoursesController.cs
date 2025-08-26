namespace Syki.Back.Features.Academic.GetCourses;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCoursesController(GetCoursesService service) : ControllerBase
{
    /// <summary>
    /// Cursos
    /// </summary>
    /// <remarks>
    /// Retorna todos os cursos.
    /// </remarks>
    [HttpGet("academic/courses")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get();
        return Ok(courses);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCoursesOut>;
