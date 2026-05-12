using Syki.Back.Auth.Policies;

namespace Syki.Back.Features.Identity.CreateRole;

[ApiController, Authorize(Policies.CreateRole)]
public class CreateRoleController(CreateRoleService service) : ControllerBase
{
    /// <summary>
    /// Criar perfil de acesso
    /// </summary>
    /// <remarks>
    /// Cria um novo perfil de acesso vinculada à organização do usuário logado.
    /// </remarks>
    [HttpPost("identity/roles")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Create([FromBody] CreateRoleIn data)
    {
        var result = await service.Create(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<CreateRoleIn>;
internal class ResponseExamples : ExamplesProvider<CreateRoleOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    InvalidRoleName,
    InvalidRoleDescription,
    InvalidPermissionsList,
    RoleNameAlreadyExists,
    InvalidRolePermissions
>;
