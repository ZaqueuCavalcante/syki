namespace Syki.Back.Features.CourseCurriculums.CreateCourseCurriculum;

[ApiController, Authorize(Policies.CreateCourseCurriculum)]
public class CreateCourseCurriculumController(CreateCourseCurriculumService service) : ControllerBase
{
    /// <summary>
    /// Criar grade curricular
    /// </summary>
    /// <remarks>
    /// Cria uma nova grade curricular.
    /// </remarks>
    [HttpPost("course-curriculums")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCourseCurriculumIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCourseCurriculumIn>;
internal class ResponseExamples : ExamplesProvider<CreateCourseCurriculumOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CourseNotFound,
    InvalidDisciplinesList,
    InvalidCourseCurriculumName
>;
