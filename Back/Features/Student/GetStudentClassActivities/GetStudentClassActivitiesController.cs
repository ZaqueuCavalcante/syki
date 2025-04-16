namespace Syki.Back.Features.Student.GetStudentClassActivities;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentClassActivitiesController(GetStudentClassActivitiesService service) : ControllerBase
{
    /// <summary>
    /// Atividades da turma
    /// </summary>
    /// <remarks>
    /// Retorna as atividades da turma especificada.
    /// </remarks>
    [HttpGet("student/classes/{classId}/activities")]
    public async Task<IActionResult> Get([FromRoute] Guid classId)
    {
        var result = await service.Get(User.Id(), classId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
