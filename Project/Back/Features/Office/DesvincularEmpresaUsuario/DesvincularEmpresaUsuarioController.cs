using Exato.Shared.Features.Office.DesvincularEmpresaUsuario;

namespace Exato.Back.Features.Office.DesvincularEmpresaUsuario;

[ApiController, Authorize(Policies.DesvincularEmpresaUsuario)]
public class DesvincularEmpresaUsuarioController(DesvincularEmpresaUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Desvincular empresa-usuário
    /// </summary>
    /// <remarks>
    /// Desfaz o vínculo empresa-usuário.
    /// </remarks>
    [HttpPut("office/empresa-usuario")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Desvincular([FromBody] DesvincularEmpresaUsuarioIn data)
    {
        var result = await service.Desvincular(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<DesvincularEmpresaUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<DesvincularEmpresaUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound,
    EmpresaNaoEncontrada,
    WebUserCompanyNotFound,
    VinculoEmpresaUsuarioNaoExiste
>;
