using Exato.Shared.Features.Office.UpdateRole;

namespace Exato.Back.Features.Office.UpdateRole;

[ApiController, Authorize(Policies.UpdateRole)]
public class UpdateRoleController(UpdateRoleService service) : ControllerBase
{
    /// <summary>
    /// Editar role
    /// </summary>
    /// <remarks>
    /// Editar a role especificada.
    /// </remarks>
    [HttpPut("office/roles/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRoleIn data)
    {
        var result = await service.Update(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<UpdateRoleIn>;
internal class ResponseExamples : ExamplesProvider<UpdateRoleOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidRoleName,
    InvalidRoleDescription,
    InvalidFeaturesList,
    RoleNotFound,
    RoleNameAlreadyExists
>;
