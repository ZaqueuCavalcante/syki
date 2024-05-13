namespace Syki.Back.Features.Student.GetStudentInsights;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentInsightsController(GetStudentInsightsService service) : ControllerBase
{
    [HttpGet("student/insights")]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get(User.Id());
        
        return Ok(insights);
    }
}
