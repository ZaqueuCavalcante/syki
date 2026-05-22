namespace Syki.Back.Features.CourseOfferings.GetCourseOfferings;

[ApiController, Authorize(Policies.GetCourseOfferings)]
public class GetCourseOfferingsController(GetCourseOfferingsService service) : ControllerBase
{
    /// <summary>
    /// Ofertas de curso
    /// </summary>
    /// <remarks>
    /// Retorna todas as ofertas de curso.
    /// </remarks>
    [HttpGet("course-offerings")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var offerings = await service.Get();
        return Ok(offerings);
    }
}

internal class ResponseExamples : ExamplesProvider<GetCourseOfferingsOut>;
