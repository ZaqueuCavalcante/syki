namespace Syki.Back.Features.Student.CreateStudentEnrollment;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateStudentEnrollmentController(CreateStudentEnrollmentService service) : ControllerBase
{
    /// <summary>
    /// Realizar matrícula
    /// </summary>
    [HttpPost("student/enrollments")]
    [ProducesResponseType<EnrollmentClassOut>(200)]
    [ProducesResponseType<SykiError>(400)]
    public async Task<IActionResult> Create([FromBody] CreateStudentEnrollmentIn data)
    {
        var x = data.Classes;
        var result = await service.Create(User.InstitutionId(), User.Id(), User.GetCourseCurriculumId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
