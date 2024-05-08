namespace Syki.Back.Features.Academic.GetCursos;

/// <summary>
/// Retorna todos os cursos da instituição.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCursosController(GetCursosService service) : ControllerBase
{
    [HttpGet("cursos")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        var cursos = await service.Get(User.InstitutionId());

        return Ok(cursos);
    }
}
