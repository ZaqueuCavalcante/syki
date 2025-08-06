namespace Syki.Back.Features.Student.GetStudentAverageNote;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentAverageNoteController(GetStudentAverageNoteService service) : ControllerBase
{
    /// <summary>
    /// Nota média
    /// </summary>
    /// <remarks>
    /// Retorna a nota média do aluno.
    /// </remarks>
    [HttpGet("student/average-note")]
    public async Task<IActionResult> Get()
    {
        var averageNote = await service.Get(User.Id);

        return Ok(new GetStudentAverageNoteOut { AverageNote = averageNote });
    }
}
