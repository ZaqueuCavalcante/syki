namespace Syki.Back.Features.Teacher.GetTeacherClassStudents;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherClassStudentsController(GetTeacherClassStudentsService service) : ControllerBase
{
    /// <summary>
    /// Notas da turma
    /// </summary>
    /// <remarks>
    /// Retorna as notas da turma informada.
    /// </remarks>
    [HttpGet("teacher/classes/{classId}/students")]
    public async Task<IActionResult> Get([FromRoute] Guid classId)
    {
        var result = await service.Get(User.Id(), classId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
