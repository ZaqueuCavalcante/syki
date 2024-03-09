namespace Syki.Back.CreateCurso;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateCursoController(ICursosService service) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CursoIn data)
    {
        var curso = await service.Create(User.InstitutionId(), data);

        return Ok(curso);
    }
}
