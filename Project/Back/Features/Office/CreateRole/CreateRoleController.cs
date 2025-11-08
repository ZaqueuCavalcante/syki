using Exato.Shared.Features.Office.CreateRole;

namespace Exato.Back.Features.Office.CreateRole;

[ApiController, Authorize(Policies.CreateRole)]
public class CreateRoleController(CreateRoleService service) : ControllerBase
{
    /// <summary>
    /// Criar role
    /// </summary>
    /// <remarks>
    /// Cria uma nova role.
    /// </remarks>
    [HttpPost("office/roles")]
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
    InvalidFeaturesList,
    EmpresaNaoEncontrada,
    RoleNameAlreadyExists
>;
