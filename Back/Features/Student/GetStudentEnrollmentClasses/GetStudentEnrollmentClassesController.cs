namespace Syki.Back.Features.Student.GetStudentEnrollmentClasses;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentEnrollmentClassesController(GetStudentEnrollmentClassesService service) : ControllerBase
{
    [HttpGet("student/enrollment-classes")]
    public async Task<IActionResult> Get()
    {
        var classes = await service.Get(User.InstitutionId(), User.Id());

        return Ok(classes);
    }
}
