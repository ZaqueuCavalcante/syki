using Exato.Shared.Features.Office.GetOrganizationRoles;

namespace Exato.Back.Features.Office.GetOrganizationRoles;

[ApiController, Authorize(Policies.GetOrganizationRoles)]
public class GetOrganizationRolesController(GetOrganizationRolesService service) : ControllerBase
{
    /// <summary>
    /// Roles da empresa
    /// </summary>
    /// <remarks>
    /// Retorna as roles da empresa especificada.
    /// </remarks>
    [HttpGet("office/empresas/{id:int}/roles")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get([FromRoute] int id, [FromQuery] GetOrganizationRolesIn data)
    {
        var roles = await service.Get(id, data);
        return Ok(roles);
    }
}

internal class RequestExamples : ExamplesProvider<GetOrganizationRolesIn>;
internal class ResponseExamples : ExamplesProvider<GetOrganizationRolesOut>;
