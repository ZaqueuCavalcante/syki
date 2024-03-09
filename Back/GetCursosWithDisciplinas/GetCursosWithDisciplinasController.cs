namespace Syki.Back.GetCursosWithDisciplinas;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetCursosWithDisciplinasController(GetCursosWithDisciplinasService service) : ControllerBase
{
    [HttpGet("cursos/with-disciplinas")]
    public async Task<IActionResult> Get()
    {
        var cursos = await service.Get(User.InstitutionId());

        return Ok(cursos);
    }
}
