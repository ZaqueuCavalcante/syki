namespace Syki.Back.Features.Student.GetStudentClassActivity;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentClassActivityController(GetStudentClassActivityService service) : ControllerBase
{
    /// <summary>
    /// Atividade
    /// </summary>
    /// <remarks>
    /// Retorna a atividade informada.
    /// </remarks>
    [HttpGet("student/classes/{id}/activities/{activityId}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromRoute] Guid activityId)
    {
        var result = await service.Get(User.Id, id, activityId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
