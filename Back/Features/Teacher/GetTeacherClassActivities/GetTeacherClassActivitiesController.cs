namespace Syki.Back.Features.Teacher.GetTeacherClassActivities;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherClassActivitiesController(GetTeacherClassActivitiesService service) : ControllerBase
{
    /// <summary>
    /// Atividades da turma
    /// </summary>
    /// <remarks>
    /// Retorna as atividades da turma informada.
    /// </remarks>
    [HttpGet("teacher/classes/{id}/activities")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.Id(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
