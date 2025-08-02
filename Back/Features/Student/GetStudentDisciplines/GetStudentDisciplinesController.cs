namespace Syki.Back.Features.Student.GetStudentDisciplines;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentDisciplinesController(GetStudentDisciplinesService service) : ControllerBase
{
    /// <summary>
    /// Disciplinas da grade
    /// </summary>
    /// <remarks>
    /// Retorna todas as disciplinas da grade curricular do curso do aluno.
    /// </remarks>
    [HttpGet("student/disciplines")]
    public async Task<IActionResult> Get()
    {
        var disciplines = await service.Get(User.Id(), User.CourseCurriculumId());

        return Ok(disciplines);
    }
}
