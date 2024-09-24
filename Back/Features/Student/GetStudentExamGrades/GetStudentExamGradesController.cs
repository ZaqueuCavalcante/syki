namespace Syki.Back.Features.Student.GetStudentExamGrades;

/// <summary>
/// Retorna todas as Avaliações vinculadas com o Aluno.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentExamGradesController(GetStudentExamGradesService service) : ControllerBase
{
    [HttpGet("student/exam-grades")]
    public async Task<IActionResult> Get()
    {
        var examGrades = await service.Get(User.Id());

        return Ok(examGrades);
    }
}
