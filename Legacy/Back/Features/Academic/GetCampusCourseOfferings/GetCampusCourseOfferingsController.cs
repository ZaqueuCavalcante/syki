namespace Estud.Back.Features.Academic.GetCampusCourseOfferings;

[ApiController, Authorize]
[EnableRateLimiting("Medium")]
public class GetCampusCourseOfferingsController(GetCampusCourseOfferingsService service) : ControllerBase
{
    /// <summary>
    /// Ofertas de curso do campus
    /// </summary>
    /// <remarks>
    /// Retorna todas as ofertas de curso do campus especificado.
    /// </remarks>
    [HttpGet("academic/campi/{id}/course-offerings")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var courseOfferings = await service.Get(id);
        return Ok(courseOfferings);
    }
}
