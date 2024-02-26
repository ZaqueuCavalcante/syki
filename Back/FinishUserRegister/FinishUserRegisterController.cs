using Syki.Shared.FinishUserRegister;

namespace Syki.Back.FinishUserRegister;

[ApiController]
[EnableRateLimiting("Small")]
public class FinishUserRegisterController(FinishUserRegisterService service) : ControllerBase
{
    [HttpPut("user-register")]
    public async Task<IActionResult> Finish([FromBody] FinishUserRegisterIn data)
    {
        await service.Finish(data);

        return Ok();
    }
}
