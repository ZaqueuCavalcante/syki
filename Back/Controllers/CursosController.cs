namespace Syki.Back.Controllers;

[ApiController, AuthAcademico]
[EnableRateLimiting("Medium")]
public class CursosController(ICursosService service) : ControllerBase
{
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var cursos = await service.GetAll(User.InstitutionId());

        return Ok(cursos);
    }

    [HttpGet("disciplinas")]
    public async Task<IActionResult> GetAllWithDisciplinas()
    {
        var cursos = await service.GetAllWithDisciplinas(User.InstitutionId());

        return Ok(cursos);
    }

    [HttpGet("{id}/disciplinas")]
    public async Task<IActionResult> GetDisciplinas([FromRoute] Guid id)
    {
        var disciplinas = await service.GetDisciplinas(id, User.InstitutionId());

        return Ok(disciplinas);
    }
}
