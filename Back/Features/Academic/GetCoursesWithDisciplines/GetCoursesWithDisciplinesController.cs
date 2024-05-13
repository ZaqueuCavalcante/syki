namespace Syki.Back.Features.Academic.GetCoursesWithDisciplines;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCoursesWithDisciplinesController(GetCoursesWithDisciplinesService service) : ControllerBase
{
    [HttpGet("academic/courses/with-disciplines")]
    public async Task<IActionResult> Get()
    {
        var courses = await service.Get(User.InstitutionId());

        return Ok(courses);
    }
}
