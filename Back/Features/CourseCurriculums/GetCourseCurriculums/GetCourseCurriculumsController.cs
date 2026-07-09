namespace Estud.Back.Features.CourseCurriculums.GetCourseCurriculums;

[ApiController, Authorize(Policies.GetCourseCurriculums)]
public class GetCourseCurriculumsController(GetCourseCurriculumsService service) : ControllerBase
{
    /// <summary>
    /// Grades curriculares
    /// </summary>
    /// <remarks>
    /// Retorna todas as grades curriculares.
    /// </remarks>
    [HttpGet("course-curriculums")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var curriculums = await service.Get();
        return Ok(curriculums);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCourseCurriculumsOut>;
