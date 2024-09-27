namespace Syki.Back.Features.Academic.CreateTeacher;

/// <summary>
/// Cria um novo Professor.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateTeacherController(CreateTeacherService service) : ControllerBase
{
    [HttpPost("academic/teachers")]
    public async Task<IActionResult> Create([FromBody] CreateTeacherIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
