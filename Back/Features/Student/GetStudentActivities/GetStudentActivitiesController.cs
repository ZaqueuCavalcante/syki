namespace Syki.Back.Features.Student.GetStudentActivities;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentActivitiesController(GetStudentActivitiesService service) : ControllerBase
{
    /// <summary>
    /// Atividades
    /// </summary>
    /// <remarks>
    /// Retorna todas as atividades do aluno.
    /// </remarks>
    [HttpGet("student/activities")]
    public async Task<IActionResult> Get()
    {
        var activities = await service.Get(User.Id());

        return Ok(activities);
    }
}
