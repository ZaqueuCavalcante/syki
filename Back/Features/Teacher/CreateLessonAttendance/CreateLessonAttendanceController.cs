namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

/// <summary>
/// Realiza a Chamada da Aula informada.
/// </summary>
[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateLessonAttendanceController(CreateLessonAttendanceService service) : ControllerBase
{
    /// <param name="id">Id da aula</param>
    /// <param name="data">Lista com os ids dos estudantes presentes na aula</param>
    [HttpPut("teacher/lessons/{id}/attendance")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateLessonAttendanceIn data)
    {
        var result = await service.Create(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
