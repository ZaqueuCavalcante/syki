namespace Estud.Back.Features.CourseCurriculums.GetCourseCurriculums;

[ApiController, Authorize(Policies.GetCourseCurriculums)]
public class GetCourseCurriculumsController(GetCourseCurriculumsService service) : ControllerBase
{
    /// <summary>
    /// Grades curriculares
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de grades curriculares da instituição, ordenadas por nome.
    /// </remarks>
    [HttpGet("course-curriculums")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetCourseCurriculumsIn query)
    {
        var curriculums = await service.Get(query);
        return Ok(curriculums);
    }
}

internal class RequestExamples : ExamplesProvider<GetCourseCurriculumsIn>;
internal class ResponseExamples : ExamplesProvider<GetCourseCurriculumsOut>;
