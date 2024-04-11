namespace Syki.Back.GetProfessorInsights;

[ApiController, AuthProfessor]
[EnableRateLimiting("Medium")]
public class GetProfessorInsightsController(GetProfessorInsightsService service) : ControllerBase
{
    [HttpGet("professor-insights")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.InstitutionId(), User.Id());
        
        return Ok(data);
    }
}
