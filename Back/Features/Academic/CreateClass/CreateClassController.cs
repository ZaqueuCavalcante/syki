namespace Syki.Back.Features.Academic.CreateClass;

/// <summary>
/// Cria uma nova turma.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateClassController(CreateClassService service) : ControllerBase
{
    [HttpPost("academic/classes")]
    [ProducesResponseType(typeof(ClassOut), 200)]
    public async Task<IActionResult> Create([FromBody] CreateClassIn data)
    {
        var @class = await service.Create(User.InstitutionId(), data);

        return Ok(@class);
    }
}
