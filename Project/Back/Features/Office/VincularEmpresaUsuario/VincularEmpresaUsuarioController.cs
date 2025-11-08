using Exato.Shared.Features.Office.VincularEmpresaUsuario;

namespace Exato.Back.Features.Office.VincularEmpresaUsuario;

[ApiController, Authorize(Policies.VincularEmpresaUsuario)]
public class VincularEmpresaUsuarioController(VincularEmpresaUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Vincular empresa-usuário
    /// </summary>
    /// <remarks>
    /// Faz o vínculo empresa-usuário.
    /// </remarks>
    [HttpPost("office/empresa-usuario")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Vincular([FromBody] VincularEmpresaUsuarioIn data)
    {
        var result = await service.Vincular(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<VincularEmpresaUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<VincularEmpresaUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound,
    EmpresaNaoEncontrada,
    UsuarioJaVinculadoAEmpresaNoIntelligence,
    UsuarioJaVinculadoAEmpresaNoExatoWeb
>;
