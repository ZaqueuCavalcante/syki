namespace Syki.Back.Features.Identity.GetRole;

[ApiController, Authorize(Policies.GetRole)]
public class GetRoleController(GetRoleService service) : ControllerBase
{
    /// <summary>
    /// Perfil de acesso
    /// </summary>
    /// <remarks>
    /// Retorna os dados de um perfil de acesso da instituição do usuário logado.
    /// </remarks>
    [HttpGet("identity/roles/{id}")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Get(int id)
    {
        var result = await service.Get(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<GetRoleOut>;
internal class ErrorsExamples : ErrorExamplesProvider<RoleNotFound>;
