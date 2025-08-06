namespace Syki.Back.Features.Teacher.GetTeacherLessonAttendances;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetTeacherLessonAttendancesController(GetTeacherLessonAttendancesService service) : ControllerBase
{
    /// <summary>
    /// Frequência da aula
    /// </summary>
    /// <remarks>
    /// Retornas a frequência da aula informada.
    /// </remarks>
    [HttpGet("teacher/lessons/{id}/attendances")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId, User.Id, id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
