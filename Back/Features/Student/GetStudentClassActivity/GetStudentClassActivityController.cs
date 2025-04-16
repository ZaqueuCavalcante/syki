namespace Syki.Back.Features.Student.GetStudentClassActivity;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentClassActivityController(GetStudentClassActivityService service) : ControllerBase
{
    /// <summary>
    /// Atividade
    /// </summary>
    /// <remarks>
    /// Retorna a atividade informada.
    /// </remarks>
    [HttpGet("student/classes/{classId}/activities/{activityId}")]
    public async Task<IActionResult> Get([FromRoute] Guid classId, [FromRoute] Guid activityId)
    {
        var result = await service.Get(User.Id(), classId, activityId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
