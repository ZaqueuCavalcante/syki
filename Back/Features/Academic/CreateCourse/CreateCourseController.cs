namespace Syki.Back.Features.Academic.CreateCourse;

/// <summary>
/// Cria um novo curso.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCourseController(CreateCourseService service) : ControllerBase
{
    [HttpPost("academic/courses")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCourseIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
