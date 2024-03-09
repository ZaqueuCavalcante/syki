using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class IndexController : ControllerBase
{
    private readonly IIndexService _service;
    public IndexController(IIndexService service) => _service = service;

    [HttpGet("adm")]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> GetAllAdm()
    {
        var data = await _service.GetAllAdm();
        
        return Ok(data);
    }

    [AuthAcademico]
    [HttpGet("academico")]
    public async Task<IActionResult> GetAllAcademico()
    {
        var data = await _service.GetAllAcademico(User.Facul());
        
        return Ok(data);
    }

    [HttpGet("aluno")]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> GetAllAluno()
    {
        var data = await _service.GetAllAluno(User.Id());
        
        return Ok(data);
    }
}
