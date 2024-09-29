namespace Syki.Back.Features.Student.GetStudentAverageNote;

/// <summary>
/// Retorna a Nota Média do Aluno.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentAverageNoteController(GetStudentAverageNoteService service) : ControllerBase
{
    [HttpGet("student/average-note")]
    public async Task<IActionResult> Get()
    {
        var averageNote = await service.Get(User.Id());

        return Ok(new GetStudentAverageNoteOut { AverageNote = averageNote });
    }
}
