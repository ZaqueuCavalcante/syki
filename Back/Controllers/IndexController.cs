using Syki.Back.Services;
using Syki.Back.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

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

    [HttpGet("academico")]
    [Authorize(Roles = Academico)]
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
