namespace Syki.Back.Features.Academic.CreateLessons;

/// <summary>
/// Cria as Aulas de uma Turma.
/// Se baseia nas datas de início e fim do Período Acadêmico.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateLessonsController(CreateLessonsService service) : ControllerBase
{
    [HttpPost("academic/classes/{id}/lessons")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromRoute] Guid id)
    {
        var result = await service.Create(User.InstitutionId(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
