namespace Syki.Back.Features.Adm.GetUsers;

/// <summary>
/// Retorna todos os Usuários.
/// </summary>
[ApiController, AuthAdm]
[Consumes("application/json"), Produces("application/json")]
public class GetUsersController(GetUsersService service) : ControllerBase
{
    [HttpGet("adm/users")]
    public async Task<IActionResult> Get()
    {
        var users = await service.Get();

        return Ok(users);
    }
}
