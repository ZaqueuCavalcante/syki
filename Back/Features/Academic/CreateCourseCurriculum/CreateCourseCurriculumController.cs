namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

/// <summary>
/// Cria uma nova Grade Curricular.
/// </summary>
[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCourseCurriculumController(CreateCourseCurriculumService service) : ControllerBase
{
    [HttpPost("academic/course-curriculums")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCourseCurriculumIn data)
    {
        var result = await service.Create(User.InstitutionId(), data);

        return result.Match<IActionResult>(Ok, BadRequest);
    }
}
