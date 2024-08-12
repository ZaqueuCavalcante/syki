namespace Syki.Back.Features.Teacher.CreateLessonAttendance;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateLessonAttendanceController(CreateLessonAttendanceService service) : ControllerBase
{
    [HttpPut("teacher/lessons/{id}/attendance")]
    public async Task<IActionResult> Create([FromRoute] Guid id, [FromBody] CreateLessonAttendanceIn data)
    {
        var result = await service.Create(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
