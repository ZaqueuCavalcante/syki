namespace Syki.Back.Features.Student.GetStudentEnrollmentClasses;

/// <summary>
/// Retorna as Turmas que o Aluno pode se matricular.
/// </summary>
[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentEnrollmentClassesController(GetStudentEnrollmentClassesService service) : ControllerBase
{
    [HttpGet("student/enrollment-classes")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId(), User.Id(), User.GetCourseCurriculumId());

        return Ok(classes);
    }
}
