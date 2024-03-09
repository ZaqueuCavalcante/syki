using Syki.Shared.CreateUser;
namespace Syki.Back.Controllers;

[EnableRateLimiting("Small")]
[ApiController, Route("[controller]")]
public class UsersController(IAuthService authService) : ControllerBase
{
    [AuthAdm]
    [HttpPost()]
    public async Task<IActionResult> Register([FromBody] CreateUserIn data)
    {
        var user = await authService.Register(data);

        return Ok(user);
    }

    [AuthAdm]
    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var users = await authService.GetAllUsers();

        return Ok(users);
    }
}
