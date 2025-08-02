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
    public async Task<IActionResult> Create([FromBody] CreateCourseOfferingIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
