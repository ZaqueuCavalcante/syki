namespace Estud.Back.Features.CourseOfferings.GetCourseOfferings;

[ApiController, Authorize(Policies.GetCourseOfferings)]
public class GetCourseOfferingsController(GetCourseOfferingsService service) : ControllerBase
{
    /// <summary>
    /// Ofertas de curso
    /// </summary>
    /// <remarks>
    /// Retorna a lista paginada de ofertas de curso da instituição.
    /// </remarks>
    [HttpGet("course-offerings")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetCourseOfferingsIn query)
    {
        var offerings = await service.Get(query);
        return Ok(offerings);
    }
}

internal class RequestExamples : ExamplesProvider<GetCourseOfferingsIn>;
internal class ResponseExamples : ExamplesProvider<GetCourseOfferingsOut>;
