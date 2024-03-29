namespace Syki.Back.FinishUserRegister;

/// <summary>
/// Finaliza o registro de um usuário.
/// </summary>
[ApiController]
[EnableRateLimiting("Small")]
[Consumes("application/json"), Produces("application/json")]
public class FinishUserRegisterController(FinishUserRegisterService service) : ControllerBase
{
    [HttpPut("users")]
    public async Task<IActionResult> Finish([FromBody] FinishUserRegisterIn data)
    {
        await service.Finish(data);

        return Ok();
    }
}
