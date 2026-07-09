namespace Estud.Back.Features.Identity.GetRoles;

[ApiController, Authorize(Policies.GetRoles)]
public class GetRolesController(GetRolesService service) : ControllerBase
{
    /// <summary>
    /// Perfis de acesso
    /// </summary>
    /// <remarks>
    /// Retorna todos os perfis de acesso da instituição do usuário logado.
    /// </remarks>
    [HttpGet("identity/roles")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public async Task<IActionResult> Get()
    {
        var roles = await service.Get();
        return Ok(roles);
    }
}

internal class ResponseExamples : ExamplesProvider<GetRolesOut>;
