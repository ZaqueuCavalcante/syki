namespace Syki.Back.RegisterUser;

[ApiController, AuthAdm]
public class RegisterUserController(RegisterUserService service) : ControllerBase
{
    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] CreateUserIn data)
    {
        var user = await service.Register(data);

        return Ok(user);
    }
}
