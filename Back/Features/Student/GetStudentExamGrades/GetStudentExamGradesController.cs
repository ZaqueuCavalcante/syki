namespace Syki.Back.Features.Student.GetStudentExamGrades;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentExamGradesController(GetStudentExamGradesService service) : ControllerBase
{
    /// <summary>
    /// Avaliações
    /// </summary>
    /// <remarks>
    /// Retorna todas as avaliações vinculadas com o aluno.
    /// </remarks>
    [HttpGet("student/exam-grades")]
    public async Task<IActionResult> Get()
    {
        var examGrades = await service.Get(User.Id());

        return Ok(examGrades);
    }
}
