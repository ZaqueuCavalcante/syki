namespace Syki.Back.Features.Academic.GetCoursesWithCurriculums;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCoursesWithCurriculumsController(GetCoursesWithCurriculumsService service) : ControllerBase
{
    /// <summary>
    /// Cursos com grades
    /// </summary>
    /// <remarks>
    /// Retorna todos os cursos que possuem grades curriculares vinculadas.
    /// </remarks>
    [HttpGet("academic/courses/with-curriculums")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId);
        return Ok(courses);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCoursesWithCurriculumsOut>;
