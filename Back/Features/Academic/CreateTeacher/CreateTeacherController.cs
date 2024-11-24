namespace Syki.Back.Features.Academic.CreateTeacher;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateTeacherController(CreateTeacherService service) : ControllerBase
{
    /// <summary>
    /// Criar professor
    /// </summary>
    /// <remarks>
    /// Cria um novo professor.
    /// </remarks>
    [HttpPost("academic/teachers")]
    public async Task<IActionResult> Create([FromBody] CreateTeacherIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
