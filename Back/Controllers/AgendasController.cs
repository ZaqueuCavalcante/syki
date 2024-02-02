using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[ApiController, Route("[controller]")]
public class AgendasController : ControllerBase
{
    private readonly IAgendasService _service;
    public AgendasController(IAgendasService service) => _service = service;

    [HttpGet("aluno")]
    [Authorize(Roles = Aluno)]
    public async Task<IActionResult> GetAluno()
    {
        var agenda = await _service.GetAluno(User.Facul(), User.Id());
        
        return Ok(agenda);
    }
}
