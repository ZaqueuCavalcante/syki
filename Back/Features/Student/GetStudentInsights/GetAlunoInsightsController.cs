namespace Syki.Back.GetAlunoInsights;

[ApiController, AuthStudent]
[EnableRateLimiting("Medium")]
public class GetAlunoInsightsController(GetAlunoInsightsService service) : ControllerBase
{
    [HttpGet("aluno-insights")]
    public async Task<IActionResult> Get()
    {
        var data = await service.Get(User.Id());
        
        return Ok(data);
    }
}
