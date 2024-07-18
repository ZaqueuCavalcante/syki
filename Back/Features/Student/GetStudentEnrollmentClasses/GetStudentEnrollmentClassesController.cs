namespace Syki.Back.Features.Student.GetStudentEnrollmentClasses;

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
