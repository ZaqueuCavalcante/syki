namespace Syki.Back.Controllers;

[ApiController, AuthAdm]
[EnableRateLimiting("Small")]
public class UsersController(IAuthService authService) : ControllerBase
{
    [HttpPost("users")]
    public async Task<IActionResult> Register([FromBody] CreateUserIn data)
    {
        var user = await authService.Register(data);

        return Ok(user);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAll()
    {
        var users = await authService.GetAllUsers();

        return Ok(users);
    }
}
