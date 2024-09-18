namespace Syki.Back.Features.Student.CreateStudentEnrollment;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateStudentEnrollmentController(CreateStudentEnrollmentService service) : ControllerBase
{
    [HttpPost("student/enrollments")]
    public async Task<IActionResult> Create([FromBody] CreateStudentEnrollmentIn data)
    {
        var result = await service.Create(User.InstitutionId(), User.Id(), User.GetCourseCurriculumId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
