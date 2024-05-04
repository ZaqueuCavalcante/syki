namespace Syki.Back.GetUsers;

[ApiController, AuthAdm]
public class GetUsersController(GetUsersService service) : ControllerBase
{
    [HttpGet("users")]
    public async Task<IActionResult> Get()
    {
        var users = await service.Get();

        return Ok(users);
    }
}
