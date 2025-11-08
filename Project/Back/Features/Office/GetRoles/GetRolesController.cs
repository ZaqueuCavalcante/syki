using Exato.Shared.Features.Office.GetRoles;

namespace Exato.Back.Features.Office.GetRoles;

[ApiController, Authorize(Policies.GetRoles)]
public class GetRolesController(GetRolesService service) : ControllerBase
{
    /// <summary>
    /// Roles
    /// </summary>
    /// <remarks>
    /// Retorna todas as roles.
    /// </remarks>
    [HttpGet("office/roles")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromQuery] GetRolesIn data)
    {
        var roles = await service.Get(data);
        return Ok(roles);
    }
}

internal class RequestExamples : ExamplesProvider<GetRolesIn>;
internal class ResponseExamples : ExamplesProvider<GetRolesOut>;
