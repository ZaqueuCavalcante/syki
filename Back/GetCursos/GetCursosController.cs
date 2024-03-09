namespace Syki.Back.GetCursos;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class GetCursosController(GetCursosService service) : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> Get()
    {
        var cursos = await service.Get(User.InstitutionId());

        return Ok(cursos);
    }
}
