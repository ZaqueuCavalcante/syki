namespace Syki.Back.Features.Academic.CreateCourseOffering;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateCourseOfferingController(CreateCourseOfferingService service) : ControllerBase
{
    /// <summary>
    /// Criar oferta de curso
    /// </summary>
    /// <remarks>
    /// Cria uma nova oferta de curso.
    /// </remarks>
    [HttpPost("academic/course-offerings")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCourseOfferingIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCourseOfferingIn>;
internal class ResponseExamples : ExamplesProvider<CourseOfferingOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    CampusNotFound,
    CourseNotFound,
    CourseCurriculumNotFound,
    AcademicPeriodNotFound,
    InvalidShift>;
