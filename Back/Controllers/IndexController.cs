namespace Syki.Back.Controllers;

[EnableRateLimiting("Medium")]
[ApiController, Route("[controller]")]
public class IndexController(IIndexService service) : ControllerBase
{
    [AuthAdm]
    [HttpGet("adm")]
    public async Task<IActionResult> GetAllAdm()
    {
        var data = await service.GetAllAdm();
        
        return Ok(data);
    }

    [AuthAluno]
    [HttpGet("aluno")]
    public async Task<IActionResult> GetAllAluno()
    {
        var data = await service.GetAllAluno(User.Id());
        return Ok(data);
    }
}
