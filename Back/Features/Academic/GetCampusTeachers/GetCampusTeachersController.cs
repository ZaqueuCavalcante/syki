namespace Syki.Back.Features.Academic.GetCampusTeachers;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCampusTeachersController(GetCampusTeachersService service) : ControllerBase
{
    /// <summary>
    /// Professores do campus
    /// </summary>
    /// <remarks>
    /// Retorna todos os professores do campus especificado.
    /// </remarks>
    [HttpGet("academic/campi/{id}/teachers")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var teachers = await service.Get(id);
        return Ok(teachers);
    }
}
