namespace Syki.Back.Features.Student.GetStudentClass;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentClassController(GetStudentClassService service) : ControllerBase
{
    /// <summary>
    /// Turma
    /// </summary>
    /// <remarks>
    /// Retorna a turma especificada pelo aluno.
    /// </remarks>
    [HttpGet("student/classes/{id}")]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var result = await service.Get(User.Id, id);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
