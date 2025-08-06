namespace Syki.Back.Features.Teacher.GetTeacherInsights;

[ApiController, AuthTeacher]
[EnableRateLimiting("Medium")]
public class GetTeacherInsightsController(GetTeacherInsightsService service) : ControllerBase
{
    /// <summary>
    /// Insights
    /// </summary>
    /// <remarks>
    /// Retorna os insights do Professor.
    /// </remarks>
    [HttpGet("teacher/insights")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId, User.Id);
        
        return Ok(data);
    }
}
