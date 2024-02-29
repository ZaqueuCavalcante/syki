using Syki.Shared.CreateUser;
using static Syki.Back.Configs.AuthorizationConfigs;

namespace Syki.Back.Controllers;

[EnableRateLimiting("Small")]
[ApiController, Route("[controller]")]
public class UsersController(IAuthService authService) : ControllerBase
{
    [HttpPost()]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> Register([FromBody] CreateUserIn data)
    {
        var user = await authService.Register(data);

        return Ok(user);
    }

    [HttpGet()]
    [Authorize(Roles = Adm)]
    public async Task<IActionResult> GetAll()
    {
        var users = await authService.GetAllUsers();

        return Ok(users);
    }
}
