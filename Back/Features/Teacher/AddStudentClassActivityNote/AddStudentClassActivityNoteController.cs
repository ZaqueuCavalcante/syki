namespace Syki.Back.Features.Teacher.AddStudentClassActivityNote;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class AddStudentClassActivityNoteController(AddStudentClassActivityNoteService service) : ControllerBase
{
    /// <summary>
    /// Atribuir nota
    /// </summary>
    /// <remarks>
    /// Atribui a nota do aluno na atividade informada.
    /// </remarks>
    [HttpPost("teacher/class-activities/{id}")]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddStudentClassActivityNoteIn data)
    {
        var result = await service.Add(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
