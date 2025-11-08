using Exato.Shared.Features.Office.GetUsers;

namespace Exato.Back.Features.Office.GetUsers;

[ApiController, Authorize(Policies.GetUsers)]
public class GetUsersController(GetUsersService service) : ControllerBase
{
    /// <summary>
    /// Usuários
    /// </summary>
    /// <remarks>
    /// Retorna os usuários.
    /// </remarks>
    [HttpGet("office/users")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetUsersIn data)
    {
        var users = await service.Get(data);
        return Ok(users);
    }
}

internal class RequestExamples : ExamplesProvider<GetUsersIn>;
internal class ResponseExamples : ExamplesProvider<GetUsersOut>;
