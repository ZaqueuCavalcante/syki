namespace Syki.Back.Features.Academic.GetClassroomAgenda;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetClassroomAgendaController(GetClassroomAgendaService service) : ControllerBase
{
    /// <summary>
    /// Agenda da sala
    /// </summary>
    /// <remarks>
    /// Retorna a agenda da sala.
    /// </remarks>
    [HttpGet("academic/classrooms/{id}/agenda")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId, id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
