namespace Syki.Back.Features.Academic.GetCourseOfferings;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCourseOfferingsController(GetCourseOfferingsService service) : ControllerBase
{
    /// <summary>
    /// Ofertas de curso
    /// </summary>
    /// <remarks>
    /// Retorna todas as ofertas de curso.
    /// </remarks>
    [HttpGet("academic/course-offerings")]
    public async Task<IActionResult> Get()
    {
        var offerings = await service.Get(User.InstitutionId());
        return Ok(offerings);
    }
}
