namespace Estud.Back.Features.Classrooms.GetClassrooms;

[ApiController, Authorize(Policies.GetClassrooms)]
public class GetClassroomsController(GetClassroomsService service) : ControllerBase
{
    /// <summary>
    /// Salas de Aula
    /// </summary>
    /// <remarks>
    /// Retorna todas as salas de aula.
    /// </remarks>
    [HttpGet("classrooms")]
    public async Task<IActionResult> Get()
    {
        var classrooms = await service.Get();
        return Ok(classrooms);
    }
}
