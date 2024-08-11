namespace Syki.Back.Features.Academic.CreateDiscipline;

/// <summary>
/// Cria uma nova disciplina.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateDisciplineController(CreateDisciplineService service) : ControllerBase
{
    [HttpPost("academic/disciplines")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateDisciplineIn data)
    {
        var discipline = await service.Create(User.InstitutionId(), data);

        return Ok(discipline);
    }
}
