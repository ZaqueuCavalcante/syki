namespace Syki.Back.Features.Student.GetStudentNotes;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentNotesController(GetStudentNotesService service) : ControllerBase
{
    /// <summary>
    /// Notas
    /// </summary>
    /// <remarks>
    /// Retorna todas as notas do aluno.
    /// </remarks>
    [HttpGet("student/notes")]
    public async Task<IActionResult> Get()
    {
        var notes = await service.Get(User.Id);

        return Ok(notes);
    }
}
