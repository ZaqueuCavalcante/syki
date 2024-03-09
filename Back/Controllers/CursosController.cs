namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CursosController(ICursosService service) : ControllerBase
{
    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var disciplinas = await service.GetDisciplinas(id, User.InstitutionId());

        return Ok(disciplinas);
    }
}
