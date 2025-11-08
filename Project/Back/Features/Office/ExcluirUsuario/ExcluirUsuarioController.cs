using Exato.Shared.Features.Office.ExcluirUsuario;

namespace Exato.Back.Features.Office.ExcluirUsuario;

[ApiController, Authorize(Policies.ExcluirUsuario)]
public class ExcluirUsuarioController(ExcluirUsuarioService service) : ControllerBase
{
    /// <summary>
    /// Excluir usuário
    /// </summary>
    /// <remarks>
    /// Exclui o usuário especificado.
    /// </remarks>
    [HttpPut("office/usuarios/{id}/excluir")]
    [SwaggerResponseExample(200, typeof(ResponseExamples))]
    [SwaggerResponseExample(400, typeof(ErrorsExamples))]
    public async Task<IActionResult> Excluir([FromRoute] int id)
    {
        var result = await service.Excluir(id);
        return result.Match<IActionResult>(Ok, BadRequest);
    }
}

internal class ResponseExamples : ExamplesProvider<ExcluirUsuarioOut>;
internal class ErrorsExamples : ErrorExamplesProvider<
    UserNotFound,
    UserAlreadyDeleted,
    EmpresaNaoEncontrada,
    WebUserCompanyNotFound,
    WebUserNotFound
>;
