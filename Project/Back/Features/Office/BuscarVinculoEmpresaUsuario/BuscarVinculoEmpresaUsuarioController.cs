using Exato.Shared.Features.Office.BuscarVinculoEmpresaUsuario;

namespace Exato.Back.Features.Office.BuscarVinculoEmpresaUsuario;

[ApiController, Authorize(Policies.BuscarVinculoEmpresaUsuario)]
public class BuscarVinculoEmpresaUsuarioController(BuscarVinculoEmpresaUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Vínculo empresa-usuário
    /// </summary>
    /// <remarks>
    /// Retorna os dados do vínculo empresa-usuário.
    /// </remarks>
    [HttpGet("office/empresa-usuario")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Buscar([FromQuery] BuscarVinculoEmpresaUsuarioIn data)
    {
        var result = await service.Buscar(data);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class RequestExamples : ExamplesProvider<BuscarVinculoEmpresaUsuarioIn>;
internal class ResponseExamples : ExamplesProvider<BuscarVinculoEmpresaUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound,
    EmpresaNaoEncontrada
>;
