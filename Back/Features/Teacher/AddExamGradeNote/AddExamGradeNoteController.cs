namespace Syki.Back.Features.Teacher.AddExamGradeNote;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class AddExamGradeNoteController(AddExamGradeNoteService service) : ControllerBase
{
    /// <summary>
    /// Definir nota
    /// </summary>
    /// <remarks>
    /// Define a nota da avaliação informada.
    /// </remarks>
    [HttpPut("teacher/exam-grades/{id}")]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddExamGradeNoteIn data)
    {
        var result = await service.Add(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
