namespace Syki.Back.Features.Teacher.AddClassActivityNote;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class AddClassActivityNoteController(AddClassActivityNoteService service) : ControllerBase
{
    /// <summary>
    /// Definir nota
    /// </summary>
    /// <remarks>
    /// Define a nota da avaliação informada.
    /// </remarks>
    [HttpPut("teacher/notes/{id}")]
    public async Task<IActionResult> Add([FromRoute] Guid id, [FromBody] AddClassActivityNoteIn data)
    {
        var result = await service.Add(User.Id(), id, data);

        return result.Match<IActionResult>(_ => NoContent(), BadRequest);
    }
}
