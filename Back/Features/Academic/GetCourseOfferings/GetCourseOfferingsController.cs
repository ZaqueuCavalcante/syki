namespace Syki.Back.Features.Academic.GetCourseOfferings;

/// <summary>
/// Retorna todas as Ofertas de Curso.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCourseOfferingsController(GetCourseOfferingsService service) : ControllerBase
{
    [HttpGet("academic/course-offerings")]
    public async Task<IActionResult> Get()
    {
        var offerings = await service.Get(User.InstitutionId());

        return Ok(offerings);
    }
}
