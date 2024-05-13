namespace Syki.Back.Features.Academic.CreateCourseOffering;

[ApiController, AuthAcademic]
[EnableRateLimiting("Medium")]
public class CreateCourseOfferingController(CreateCourseOfferingService service) : ControllerBase
{
    [HttpPost("academic/course-offerings")]
    public async Task<IActionResult> Create([FromBody] CreateCourseOfferingIn data)
    {
        var courseOffering = await service.Create(User.InstitutionId(), data);

        return Ok(courseOffering);
    }
}
