namespace Syki.Back.Features.Academic.GetClassrooms;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetClassroomsController(GetClassroomsService service) : ControllerBase
{
    /// <summary>
    /// Salas de Aula
    /// </summary>
    /// <remarks>
    /// Retorna todas as salas de aula.
    /// </remarks>
    [HttpGet("academic/classrooms")]
    public async Task<IActionResult> Get()
    {
        var classrooms = await service.Get(User.InstitutionId);
        return Ok(classrooms);
    }
}
