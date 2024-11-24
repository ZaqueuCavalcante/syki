namespace Syki.Back.Features.Academic.CreateDiscipline;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateDisciplineController(CreateDisciplineService service) : ControllerBase
{
    /// <summary>
    /// Criar disciplina
    /// </summary>
    /// <remarks>
    /// Cria uma nova disciplina.
    /// </remarks>
    [HttpPost("academic/disciplines")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateDisciplineIn data)
    {
        var discipline = await service.Create(User.InstitutionId(), data);

        return Ok(discipline);
    }
}
