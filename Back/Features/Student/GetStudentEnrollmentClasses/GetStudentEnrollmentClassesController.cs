namespace Syki.Back.Features.Student.GetStudentEnrollmentClasses;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentEnrollmentClassesController(GetStudentEnrollmentClassesService service) : ControllerBase
{
    /// <summary>
    /// Turmas para matrícula
    /// </summary>
    /// <remarks>
    /// Retorna as turmas que o aluno pode se matricular.
    /// </remarks>
    [HttpGet("student/enrollment-classes")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId, User.Id, User.CourseCurriculumId);

        return Ok(classes);
    }
}
