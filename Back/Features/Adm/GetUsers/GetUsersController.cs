namespace Syki.Back.Features.Adm.GetUsers;

[ApiController, AuthAdm]
public class GetUsersController(GetUsersService service) : ControllerBase
{
    /// <summary>
    /// Usuários
    /// </summary>
    /// <remarks>
    /// Retorna todos os usuários.
    /// </remarks>
    [HttpGet("adm/users")]
    public async Task<IActionResult> Get()
    {
        var users = await service.Get();

        return Ok(users);
    }
}
