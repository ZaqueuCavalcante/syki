namespace Syki.Back.Features.Student.GetStudentInsights;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentInsightsController(GetStudentInsightsService service) : ControllerBase
{
    [HttpGet("student/insights")]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get(User.GetCourseCurriculumId());
        
        return Ok(insights);
    }
}
