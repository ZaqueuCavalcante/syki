namespace Syki.Back.Features.Student.GetStudentClassActivities;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentClassActivitiesController(GetStudentClassActivitiesService service) : ControllerBase
{
    /// <summary>
    /// Atividades da turma
    /// </summary>
    /// <remarks>
    /// Retorna as atividades da turma especificada.
    /// </remarks>
    [HttpGet("student/classes/{id}/activities")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.Id(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
