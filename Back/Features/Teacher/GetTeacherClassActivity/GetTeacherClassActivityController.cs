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
    [HttpGet("teacher/classes/{classId}/activities/{activityId}")]
    public async Task<IActionResult> Get([FromRoute] Guid classId, [FromRoute] Guid activityId)
    {
        var result = await service.Get(User.Id(), classId, activityId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
