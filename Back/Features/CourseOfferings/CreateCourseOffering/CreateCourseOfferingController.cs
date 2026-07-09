namespace Estud.Back.Features.CourseOfferings.CreateCourseOffering;

[ApiController, Authorize(Policies.CreateCourseOffering)]
public class CreateCourseOfferingController(CreateCourseOfferingService service) : ControllerBase
{
    /// <summary>
    /// Criar oferta de curso
    /// </summary>
    /// <remarks>
    /// Cria uma nova oferta de curso.
    /// </remarks>
    [HttpPost("course-offerings")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateCourseOfferingIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateCourseOfferingIn>;
internal class ResponseExamples : ExamplesProvider<CourseOfferingOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidCourseSession,
    CampusNotFound,
    CourseNotFound,
    CourseCurriculumNotFound,
    AcademicPeriodNotFound
>;
