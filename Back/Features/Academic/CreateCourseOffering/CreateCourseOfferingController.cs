namespace Syki.Back.Features.Academic.CreateCourseOffering;

/// <summary>
/// Cria uma nova Oferta de Curso.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCourseOfferingController(CreateCourseOfferingService service) : ControllerBase
{
    [HttpPost("academic/course-offerings")]
    public async Task<IActionResult> Create([FromBody] CreateCourseOfferingIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
