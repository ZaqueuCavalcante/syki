namespace Syki.Back.Features.Academic.GetCoursesWithCurriculums;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCoursesWithCurriculumsController(GetCoursesWithCurriculumsService service) : ControllerBase
{
    [HttpGet("academic/courses/with-curriculums")]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
