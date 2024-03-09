namespace Syki.Back.CreateCurso;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CreateCursoController(CreateCursoService service) : ControllerBase
{
    [HttpPost("cursos")]
    public async Task<IActionResult> Create([FromBody] CursoIn data)
    {
        var curso = await service.Create(User.InstitutionId(), data);

        return Ok(curso);
    }
}
