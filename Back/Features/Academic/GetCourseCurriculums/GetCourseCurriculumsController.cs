namespace Syki.Back.Features.Academic.GetCourseCurriculums;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetCourseCurriculumsController(GetCourseCurriculumsService service) : ControllerBase
{
    /// <summary>
    /// Grades curriculares
    /// </summary>
    /// <remarks>
    /// Retorna todas as grades curriculares.
    /// </remarks>
    [HttpGet("academic/course-curriculums")]
    public async Task<IActionResult> Get()
    {
        var courseCurriculums = await service.Get(User.InstitutionId());

        return Ok(courseCurriculums);
    }
}
