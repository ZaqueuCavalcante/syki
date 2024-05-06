namespace Syki.Back.Features.Academic.CreateCurso;

/// <summary>
/// Cria um novo curso.
/// </summary>
[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCursoController(CreateCursoService service) : ControllerBase
{
    [HttpPost("cursos")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CourseIn data)
    {
        var curso = await service.Create(User.InstitutionId(), data);

        return Ok(curso);
    }
}
