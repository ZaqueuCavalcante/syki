using Exato.Shared.Features.Office.VincularEmpresasAoUsuario;

namespace Exato.Back.Features.Office.VincularEmpresasAoUsuario;

[ApiController, Authorize(Policies.VincularEmpresasAoUsuario)]
public class VincularEmpresasAoUsuarioController(VincularEmpresasAoUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Vincular empresas ao usuário
    /// </summary>
    /// <remarks>
    /// Faz o vínculo das empresas informadas com o usuário.
    /// </remarks>
    [HttpPost("office/usuarios/{id}/empresas")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Vincular([FromRoute] int id, [FromBody] VincularEmpresasAoUsuarioIn data)
    {
        var result = await service.Vincular(id, data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<VincularEmpresasAoUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<VincularEmpresasAoUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound,
    EmpresaNaoEncontrada,
    UsuarioJaVinculadoAEmpresaNoIntelligence,
    UsuarioJaVinculadoAEmpresaNoExatoWeb
>;
