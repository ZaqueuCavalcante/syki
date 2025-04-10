namespace Syki.Back.Features.Student.GetStudentClass;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentClassController(GetStudentClassService service) : ControllerBase
{
    /// <summary>
    /// Turma
    /// </summary>
    /// <remarks>
    /// Retorna a turma especificada pelo aluno.
    /// </remarks>
    [HttpGet("student/classes/{classId}")]
    public async Task<IActionResult> Get([FromRoute] Guid classId)
    {
        var result = await service.Get(User.Id(), classId);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
