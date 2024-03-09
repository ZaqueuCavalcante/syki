namespace Syki.Back.GetCursoDisciplinas;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetCursoDisciplinasController(GetCursoDisciplinasService service) : ControllerBase
{
    [HttpGet("cursos/{id}/disciplinas")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var disciplinas = await service.Get(id, User.InstitutionId());

        return Ok(disciplinas);
    }
}
