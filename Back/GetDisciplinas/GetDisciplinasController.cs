namespace Syki.Back.GetDisciplinas;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetDisciplinasController(GetDisciplinasService service) : ControllerBase
{
    [HttpGet("disciplinas")]
    public async Task<IActionResult> Get([FromQuery] Guid? cursoId)
    {
        var disciplinas = await service.Get(User.InstitutionId(), cursoId);

        return Ok(disciplinas);
    }
}
