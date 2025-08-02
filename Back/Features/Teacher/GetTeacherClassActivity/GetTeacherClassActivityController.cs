namespace Syki.Back.Features.Teacher.GetTeacherClassActivity;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherClassActivityController(GetTeacherClassActivityService service) : ControllerBase
{
    /// <summary>
    /// Atividade
    /// </summary>
    /// <remarks>
    /// Retorna a atividade informada.
    /// </remarks>
    [HttpGet("teacher/classes/{id}/activities/{activityId}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] Guid activityId)
    {
        var result = await service.Get(User.Id(), id, activityId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
