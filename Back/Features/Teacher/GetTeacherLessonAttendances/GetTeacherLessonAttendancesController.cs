namespace Syki.Back.Features.Teacher.GetTeacherLessonAttendances;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherLessonAttendancesController(GetTeacherLessonAttendancesService service) : ControllerBase
{
    [HttpGet("teacher/lessons/{id}/attendances")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId(), User.Id(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
