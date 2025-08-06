namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateCourseCurriculumController(CreateCourseCurriculumService service) : ControllerBase
{
    /// <summary>
    /// Criar grade curricular
    /// </summary>
    /// <remarks>
    /// Cria uma nova grade curricular.
    /// </remarks>
    [HttpPost("academic/course-curriculums")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCourseCurriculumIn data)
    {
        var result = await service.Create(User.InstitutionId, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCourseCurriculumIn>;
internal class ResponseExamples : ExamplesProvider<CourseCurriculumOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseNotFound,
    InvalidDisciplinesList>;
