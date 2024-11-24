namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateLessonAttendanceController(CreateLessonAttendanceService service) : ControllerBase
{
    /// <summary>
    /// Realizar chamada
    /// </summary>
    /// <remarks>
    /// Realiza a chamada da aula informada.
    /// </remarks>
    [HttpPut("teacher/lessons/{id}/attendance")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateLessonAttendanceIn data)
    {
        var result = await service.Create(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
