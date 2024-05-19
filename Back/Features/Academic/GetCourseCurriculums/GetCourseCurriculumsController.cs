namespace Syki.Back.Features.Academic.GetCourseCurriculums;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCourseCurriculumsController(GetCourseCurriculumsService service) : ControllerBase
{
    [HttpGet("academic/course-curriculums")]
    public async Task<IActionResult> Get()
    {
        var courseCurriculums = await service.Get(User.InstitutionId());

        return Ok(courseCurriculums);
    }
}
