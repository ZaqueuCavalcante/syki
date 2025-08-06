namespace Syki.Back.Features.Student.CreateStudentEnrollment;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class CreateStudentEnrollmentController(CreateStudentEnrollmentService service) : ControllerBase
{
    /// <summary>
    /// Realizar matrícula
    /// </summary>
    /// <remarks>
    /// Realiza a matrícula do aluno nas turmas informadas.
    /// </remarks>
    [HttpPost("student/enrollments")]
    [ProducesResponseType<EnrollmentClassOut>(200)]
    [ProducesResponseType<SykiError>(400)]
    public async Task<IActionResult> Create([FromBody] CreateStudentEnrollmentIn data)
    {
        var result = await service.Create(User.InstitutionId, User.Id, User.CourseCurriculumId, data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
