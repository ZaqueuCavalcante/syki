namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class AgendasController : ControllerBase
{
    private readonly IAgendasService _service;
    public AgendasController(IAgendasService service) => _service = service;

    [AuthAluno]
    [HttpGet("aluno")]
    public async Task<IActionResult> GetAluno()
    {
        var agenda = await _service.GetAluno(User.Facul(), User.Id());
        
        return Ok(agenda);
    }
}
