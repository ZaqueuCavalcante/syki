namespace Syki.Back.Features.Teacher.GetTeacherInsights;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
[Consumes("application/json"), Produces("application/json")]
public class GetTeacherInsightsController(GetTeacherInsightsService service) : ControllerBase
{
    [HttpGet("teacher/insights")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(data);
    }
}
