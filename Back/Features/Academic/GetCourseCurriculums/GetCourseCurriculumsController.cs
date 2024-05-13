namespace Syki.Back.Features.Academic.GetCourseCurriculums;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class GetCourseCurriculumsController(GetCourseCurriculumsService service) : ControllerBase
{
    [HttpGet("academic/course-curriculums")]
    public async Task<IActionResult> Get()
    {
        var courseCurriculums = await service.Get(User.InstitutionId());

        return Ok(courseCurriculums);
    }
}
