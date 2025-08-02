namespace Syki.Back.Features.Teacher.GetTeacherClass;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetTeacherClassController(GetTeacherClassService service) : ControllerBase
{
    /// <summary>
    /// Turma
    /// </summary>
    /// <remarks>
    /// Retorna os dados da turma informada.
    /// </remarks>
    [HttpGet("teacher/classes/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.InstitutionId(), User.Id(), id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
