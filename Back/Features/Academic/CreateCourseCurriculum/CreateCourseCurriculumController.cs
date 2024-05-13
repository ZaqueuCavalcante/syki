namespace Syki.Back.Features.Academic.CreateCourseCurriculum;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class CreateCourseCurriculumController(CreateCourseCurriculumService service) : ControllerBase
{
    [HttpPost("academic/course-curriculums")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Create([FromBody] CreateCourseCurriculumIn data)
    {
        var courseCurriculum = await service.Create(User.InstitutionId(), data);

        return Ok(courseCurriculum);
    }
}
