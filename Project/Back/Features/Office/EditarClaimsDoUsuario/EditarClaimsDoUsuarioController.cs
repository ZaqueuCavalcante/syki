using Exato.Shared.Features.Office.EditarClaimsDoUsuario;

namespace Exato.Back.Features.Office.EditarClaimsDoUsuario;

[ApiController, Authorize(Policies.EditarClaimsDoUsuario)]
public class EditarClaimsDoUsuarioController(EditarClaimsDoUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Editar claims do usuário
    /// </summary>
    /// <remarks>
    /// Edita as claims do usuário.
    /// </remarks>
    [HttpPut("office/usuarios/{id:int}/claims")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Editar([FromRoute] int id, [FromBody] EditarClaimsDoUsuarioIn data)
    {
        var result = await service.Editar(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<EditarClaimsDoUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<EditarClaimsDoUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound
>;
