namespace Syki.Back.GetDisciplinas;

/// <summary>
/// Retorna todas as disciplinas.
/// </summary>
[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetDisciplinasController(GetDisciplinasService service) : ControllerBase
{
    [HttpGet("disciplinas")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get([FromQuery] Guid? cursoId)
    {
        var disciplinas = await service.Get(User.InstitutionId(), cursoId);

        return Ok(disciplinas);
    }
}
