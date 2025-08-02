namespace Syki.Back.Features.Student.GetStudentInsights;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetStudentInsightsController(GetStudentInsightsService service) : ControllerBase
{
    /// <summary>
    /// Insights
    /// </summary>
    /// <remarks>
    /// Retorna os insights do Aluno.
    /// </remarks>
    [HttpGet("student/insights")]
    public async Task<IActionResult> Get()
    {
        var insights = await service.Get(User.Id(), User.CourseCurriculumId());
        
        return Ok(insights);
    }
}
