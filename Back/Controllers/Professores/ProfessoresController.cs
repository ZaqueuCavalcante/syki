using Microsoft.AspNetCore.Mvc;

namespace Syki.Controllers;

[ApiController, Route("[controller]")]
public class ProfessoresController : ControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        await Task.Delay(1);

        return Ok("Todos os PROFESSORES lalala...");
    }
}
