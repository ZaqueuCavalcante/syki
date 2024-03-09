namespace Syki.Back.CreatePendingUserRegister;

[ApiController]
[EnableRateLimiting("VerySmall")]
public class CreatePendingUserRegisterController(CreatePendingUserRegisterService service) : ControllerBase
{
    [HttpPost("user-register")]
    public async Task<IActionResult> Create([FromBody] CreatePendingUserRegisterIn data)
    {
        await service.Create(data);

        return Ok();
    }
}
