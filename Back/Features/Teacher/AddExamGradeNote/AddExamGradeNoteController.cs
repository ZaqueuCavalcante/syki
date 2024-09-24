namespace Syki.Back.Features.Teacher.AddExamGradeNote;

/// <summary>
/// Define a Nota da Avaliação informada.
/// </summary>
[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class AddExamGradeNoteController(AddExamGradeNoteService service) : ControllerBase
{
    [HttpPut("teacher/exam-grades/{id}")]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddExamGradeNoteIn data)
    {
        var result = await service.Add(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
