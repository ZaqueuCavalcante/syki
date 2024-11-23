namespace Syki.Back.Features.Student.GetStudentInsights;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetStudentInsightsController(GetStudentInsightsService service) : ControllerBase
{
    /// <summary>
    /// Insights
    /// </summary>
    [HttpGet("student/insights")]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get(User.Id(), User.GetCourseCurriculumId());
        
        return Ok(insights);
    }
}
