namespace Syki.Back.Features.Student.GetStudentDisciplines;

/// <summary>
/// Retorna todas as Disciplinas da Grade Curricular do Curso do Aluno.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentDisciplinesController(GetStudentDisciplinesService service) : ControllerBase
{
    [HttpGet("student/disciplines")]
    public async Task<IActionResult> Get()
    {
        var disciplines = await service.Get(User.Id(), User.GetCourseCurriculumId());

        return Ok(disciplines);
    }
}
