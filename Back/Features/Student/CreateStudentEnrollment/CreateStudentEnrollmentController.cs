namespace Syki.Back.Features.Student.CreateStudentEnrollment;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class CreateStudentEnrollmentController(CreateStudentEnrollmentService service) : ControllerBase
{
    [HttpPost("student/enrollments")]
    public async Task<IActionResult> Create([FromBody] CreateStudentEnrollmentIn data)
    {
        await service.Create(User.InstitutionId(), User.Id(), data);

        return Ok();
    }
}
