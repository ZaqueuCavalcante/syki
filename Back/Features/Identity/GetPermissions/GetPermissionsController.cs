namespace Syki.Back.Features.Identity.GetPermissions;

[ApiController, Authorize(Policies.GetPermissions)]
public class GetPermissionsController(GetPermissionsService service) : ControllerBase
{
    /// <summary>
    /// Permissões disponíveis
    /// </summary>
    /// <remarks>
    /// Retorna todas as permissões do sistema com seus tipos de usuário permitidos.
    /// </remarks>
    [HttpGet("identity/permissions")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    public IActionResult Get()
    {
        return Ok(service.Get());
    }
}

internal class ResponseExamples : ExamplesProvider<GetPermissionsOut>;
